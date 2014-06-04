
using System;
using System.IO;
using System.Collections;

namespace Stratageme15.Core.Tools.JavascriptParser {

public class Token {
	public int kind;    // token kind
	public int pos;     // token position in bytes in the source text (starting at 0)
	public int charPos;  // token position in characters in the source text (starting at 0)
	public int col;     // token column (starting at 1)
	public int line;    // token line (starting at 1)
	public string val;  // token value
	public Token next;  // ML 2005-03-11 Tokens are kept in linked list

	public override string ToString(){
		return string.Format("[l{0}][c{1}]: {2} ({3})",line,col,val,kind);
	}
}

//-----------------------------------------------------------------------------------
// Buffer
//-----------------------------------------------------------------------------------
public class Buffer {
	// This Buffer supports the following cases:
	// 1) seekable stream (file)
	//    a) whole stream in buffer
	//    b) part of stream in buffer
	// 2) non seekable stream (network, console)

	public const int EOF = char.MaxValue + 1;
	const int MIN_BUFFER_LENGTH = 1024; // 1KB
	const int MAX_BUFFER_LENGTH = MIN_BUFFER_LENGTH * 64; // 64KB
	byte[] buf;         // input buffer
	int bufStart;       // position of first byte in buffer relative to input stream
	int bufLen;         // length of buffer
	int fileLen;        // length of input stream (may change if the stream is no file)
	int bufPos;         // current position in buffer
	Stream stream;      // input stream (seekable)
	bool isUserStream;  // was the stream opened by the user?
	
	public Buffer (Stream s, bool isUserStream) {
		stream = s; this.isUserStream = isUserStream;
		
		if (stream.CanSeek) {
			fileLen = (int) stream.Length;
			bufLen = Math.Min(fileLen, MAX_BUFFER_LENGTH);
			bufStart = Int32.MaxValue; // nothing in the buffer so far
		} else {
			fileLen = bufLen = bufStart = 0;
		}

		buf = new byte[(bufLen>0) ? bufLen : MIN_BUFFER_LENGTH];
		if (fileLen > 0) Pos = 0; // setup buffer to position 0 (start)
		else bufPos = 0; // index 0 is already after the file, thus Pos = 0 is invalid
		if (bufLen == fileLen && stream.CanSeek) Close();
	}
	
	protected Buffer(Buffer b) { // called in UTF8Buffer constructor
		buf = b.buf;
		bufStart = b.bufStart;
		bufLen = b.bufLen;
		fileLen = b.fileLen;
		bufPos = b.bufPos;
		stream = b.stream;
		// keep destructor from closing the stream
		b.stream = null;
		isUserStream = b.isUserStream;
	}

	~Buffer() { Close(); }
	
	protected void Close() {
		if (!isUserStream && stream != null) {
			stream.Close();
			stream = null;
		}
	}
	
	public virtual int Read () {
		if (bufPos < bufLen) {
			return buf[bufPos++];
		} else if (Pos < fileLen) {
			Pos = Pos; // shift buffer start to Pos
			return buf[bufPos++];
		} else if (stream != null && !stream.CanSeek && ReadNextStreamChunk() > 0) {
			return buf[bufPos++];
		} else {
			return EOF;
		}
	}

	public int Peek () {
		int curPos = Pos;
		int ch = Read();
		Pos = curPos;
		return ch;
	}
	
	// beg .. begin, zero-based, inclusive, in byte
	// end .. end, zero-based, exclusive, in byte
	public string GetString (int beg, int end) {
		int len = 0;
		char[] buf = new char[end - beg];
		int oldPos = Pos;
		Pos = beg;
		while (Pos < end) buf[len++] = (char) Read();
		Pos = oldPos;
		return new String(buf, 0, len);
	}

	public int Pos {
		get { return bufPos + bufStart; }
		set {
			if (value >= fileLen && stream != null && !stream.CanSeek) {
				// Wanted position is after buffer and the stream
				// is not seek-able e.g. network or console,
				// thus we have to read the stream manually till
				// the wanted position is in sight.
				while (value >= fileLen && ReadNextStreamChunk() > 0);
			}

			if (value < 0 || value > fileLen) {
				throw new FatalError("buffer out of bounds access, position: " + value);
			}

			if (value >= bufStart && value < bufStart + bufLen) { // already in buffer
				bufPos = value - bufStart;
			} else if (stream != null) { // must be swapped in
				stream.Seek(value, SeekOrigin.Begin);
				bufLen = stream.Read(buf, 0, buf.Length);
				bufStart = value; bufPos = 0;
			} else {
				// set the position to the end of the file, Pos will return fileLen.
				bufPos = fileLen - bufStart;
			}
		}
	}
	
	// Read the next chunk of bytes from the stream, increases the buffer
	// if needed and updates the fields fileLen and bufLen.
	// Returns the number of bytes read.
	private int ReadNextStreamChunk() {
		int free = buf.Length - bufLen;
		if (free == 0) {
			// in the case of a growing input stream
			// we can neither seek in the stream, nor can we
			// foresee the maximum length, thus we must adapt
			// the buffer size on demand.
			byte[] newBuf = new byte[bufLen * 2];
			Array.Copy(buf, newBuf, bufLen);
			buf = newBuf;
			free = bufLen;
		}
		int read = stream.Read(buf, bufLen, free);
		if (read > 0) {
			fileLen = bufLen = (bufLen + read);
			return read;
		}
		// end of stream reached
		return 0;
	}
}

//-----------------------------------------------------------------------------------
// UTF8Buffer
//-----------------------------------------------------------------------------------
public class UTF8Buffer: Buffer {
	public UTF8Buffer(Buffer b): base(b) {}

	public override int Read() {
		int ch;
		do {
			ch = base.Read();
			// until we find a utf8 start (0xxxxxxx or 11xxxxxx)
		} while ((ch >= 128) && ((ch & 0xC0) != 0xC0) && (ch != EOF));
		if (ch < 128 || ch == EOF) {
			// nothing to do, first 127 chars are the same in ascii and utf8
			// 0xxxxxxx or end of file character
		} else if ((ch & 0xF0) == 0xF0) {
			// 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
			int c1 = ch & 0x07; ch = base.Read();
			int c2 = ch & 0x3F; ch = base.Read();
			int c3 = ch & 0x3F; ch = base.Read();
			int c4 = ch & 0x3F;
			ch = (((((c1 << 6) | c2) << 6) | c3) << 6) | c4;
		} else if ((ch & 0xE0) == 0xE0) {
			// 1110xxxx 10xxxxxx 10xxxxxx
			int c1 = ch & 0x0F; ch = base.Read();
			int c2 = ch & 0x3F; ch = base.Read();
			int c3 = ch & 0x3F;
			ch = (((c1 << 6) | c2) << 6) | c3;
		} else if ((ch & 0xC0) == 0xC0) {
			// 110xxxxx 10xxxxxx
			int c1 = ch & 0x1F; ch = base.Read();
			int c2 = ch & 0x3F;
			ch = (c1 << 6) | c2;
		}
		return ch;
	}
}

//-----------------------------------------------------------------------------------
// Scanner
//-----------------------------------------------------------------------------------
public class Scanner {
	const char EOL = '\n';
	const int eofSym = 0; /* pdt */
	const int maxT = 85;
	const int noSym = 85;


	public Buffer buffer; // scanner buffer
	
	Token t;          // current token
	int ch;           // current input character
	int pos;          // byte position of current character
	int charPos;      // position by unicode characters starting with 0
	int col;          // column number of current character
	int line;         // line number of current character
	int oldEols;      // EOLs that appeared in a comment;
	static readonly Hashtable start; // maps first token character to start state

	Token tokens;     // list of tokens already peeked (first token is a dummy)
	Token pt;         // current peek token
	
	char[] tval = new char[128]; // text of current token
	int tlen;         // length of current token
	
	static Scanner() {
		start = new Hashtable(128);
		for (int i = 36; i <= 36; ++i) start[i] = 1;
		for (int i = 65; i <= 90; ++i) start[i] = 1;
		for (int i = 95; i <= 95; ++i) start[i] = 1;
		for (int i = 97; i <= 122; ++i) start[i] = 1;
		for (int i = 170; i <= 170; ++i) start[i] = 1;
		for (int i = 181; i <= 181; ++i) start[i] = 1;
		for (int i = 186; i <= 186; ++i) start[i] = 1;
		for (int i = 192; i <= 214; ++i) start[i] = 1;
		for (int i = 216; i <= 246; ++i) start[i] = 1;
		for (int i = 248; i <= 255; ++i) start[i] = 1;
		for (int i = 49; i <= 57; ++i) start[i] = 63;
		start[47] = 64; 
		start[46] = 65; 
		start[48] = 66; 
		start[34] = 26; 
		start[39] = 29; 
		start[43] = 67; 
		start[45] = 68; 
		start[42] = 69; 
		start[37] = 70; 
		start[124] = 71; 
		start[38] = 72; 
		start[94] = 73; 
		start[60] = 74; 
		start[62] = 75; 
		start[126] = 45; 
		start[33] = 76; 
		start[61] = 77; 
		start[58] = 53; 
		start[59] = 54; 
		start[44] = 55; 
		start[63] = 56; 
		start[123] = 57; 
		start[91] = 58; 
		start[40] = 59; 
		start[125] = 60; 
		start[93] = 61; 
		start[41] = 62; 
		start[Buffer.EOF] = -1;

	}
	
	public Scanner (string fileName) {
		try {
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			buffer = new Buffer(stream, false);
			Init();
		} catch (IOException) {
			throw new FatalError("Cannot open file " + fileName);
		}
	}
	
	public Scanner (Stream s) {
		buffer = new Buffer(s, true);
		Init();
	}
	
	void Init() {
		pos = -1; line = 1; col = 0; charPos = -1;
		oldEols = 0;
		NextCh();
		if (ch == 0xEF) { // check optional byte order mark for UTF-8
			NextCh(); int ch1 = ch;
			NextCh(); int ch2 = ch;
			if (ch1 != 0xBB || ch2 != 0xBF) {
				throw new FatalError(String.Format("illegal byte order mark: EF {0,2:X} {1,2:X}", ch1, ch2));
			}
			buffer = new UTF8Buffer(buffer); col = 0; charPos = -1;
			NextCh();
		}
		pt = tokens = new Token();  // first token is a dummy
	}
	
	void NextCh() {
		if (oldEols > 0) { ch = EOL; oldEols--; } 
		else {
			pos = buffer.Pos;
			// buffer reads unicode chars, if UTF8 has been detected
			ch = buffer.Read(); col++; charPos++;
			// replace isolated '\r' by '\n' in order to make
			// eol handling uniform across Windows, Unix and Mac
			if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
			if (ch == EOL) { line++; col = 0; }
		}

	}

	void AddCh() {
		if (tlen >= tval.Length) {
			char[] newBuf = new char[2 * tval.Length];
			Array.Copy(tval, 0, newBuf, 0, tval.Length);
			tval = newBuf;
		}
		if (ch != Buffer.EOF) {
			tval[tlen++] = (char) ch;
			NextCh();
		}
	}



	bool Comment0() {
		int level = 1, pos0 = pos, line0 = line, col0 = col, charPos0 = charPos;
		NextCh();
		if (ch == '/') {
			NextCh();
			for(;;) {
				if (ch == 10) {
					level--;
					if (level == 0) { oldEols = line - line0; NextCh(); return true; }
					NextCh();
				} else if (ch == Buffer.EOF) return false;
				else NextCh();
			}
		} else {
			buffer.Pos = pos0; NextCh(); line = line0; col = col0; charPos = charPos0;
		}
		return false;
	}

	bool Comment1() {
		int level = 1, pos0 = pos, line0 = line, col0 = col, charPos0 = charPos;
		NextCh();
		if (ch == '*') {
			NextCh();
			for(;;) {
				if (ch == '*') {
					NextCh();
					if (ch == '/') {
						level--;
						if (level == 0) { oldEols = line - line0; NextCh(); return true; }
						NextCh();
					}
				} else if (ch == Buffer.EOF) return false;
				else NextCh();
			}
		} else {
			buffer.Pos = pos0; NextCh(); line = line0; col = col0; charPos = charPos0;
		}
		return false;
	}


	void CheckLiteral() {
		switch (t.val) {
			case "break": t.kind = 5; break;
			case "case": t.kind = 6; break;
			case "catch": t.kind = 7; break;
			case "continue": t.kind = 8; break;
			case "debugger": t.kind = 9; break;
			case "default": t.kind = 10; break;
			case "delete": t.kind = 11; break;
			case "do": t.kind = 12; break;
			case "else": t.kind = 13; break;
			case "finally": t.kind = 14; break;
			case "for": t.kind = 15; break;
			case "function": t.kind = 16; break;
			case "if": t.kind = 17; break;
			case "in": t.kind = 18; break;
			case "instanceof": t.kind = 19; break;
			case "new": t.kind = 20; break;
			case "of": t.kind = 21; break;
			case "return": t.kind = 22; break;
			case "switch": t.kind = 23; break;
			case "this": t.kind = 24; break;
			case "throw": t.kind = 25; break;
			case "try": t.kind = 26; break;
			case "typeof": t.kind = 27; break;
			case "true": t.kind = 28; break;
			case "false": t.kind = 29; break;
			case "var": t.kind = 30; break;
			case "void": t.kind = 31; break;
			case "while": t.kind = 32; break;
			case "with": t.kind = 33; break;
			case "null": t.kind = 83; break;
			case "let": t.kind = 84; break;
			default: break;
		}
	}

	public bool _forceDivider = false;
	public Token WarrantyReturnRegexl(Token t){
		t.next = null;
		buffer.Pos = t.pos;
		line = t.line; col = t.col; charPos = t.charPos;
		NextCh();
		tlen=0;
		_forceDivider = true;
		var t2 = NextToken();
		_forceDivider = false;
		return t2;
	}

	Token NextToken() {
		while (ch == ' ' ||
			ch >= 9 && ch <= 10 || ch == 13
		) NextCh();
		if (ch == '/' && Comment0() ||ch == '/' && Comment1()) return NextToken();
		int apx = 0;
		int recKind = noSym;
		int recEnd = pos;
		t = new Token();
		t.pos = pos; t.col = col; t.line = line; t.charPos = charPos;
		if (_forceDivider && ch == '/') { 
		    t.kind = 37;
            t.val ="/";
			AddCh();
			if (ch=='=')
            {
                t.pos = pos; t.col = col; t.line = line; t.charPos = charPos;
                t.kind = 44;
                t.val = "/=";
                AddCh();
            }
            return t;
		}
		int state;
		if (start.ContainsKey(ch)) { state = (int) start[ch]; }
		else { state = 0; }
		tlen = 0; AddCh();
		
		switch (state) {
			case -1: { t.kind = eofSym; break; } // NextCh already done
			case 0: {
				if (recKind != noSym) {
					tlen = recEnd - t.pos;
					SetScannerBehindT();
				}
				t.kind = recKind; break;
			} // NextCh already done
			case 1:
				recEnd = pos; recKind = 1;
				if (ch == '$' || ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z' || ch == 128 || ch >= 160 && ch <= 179 || ch == 181 || ch == 186 || ch >= 192 && ch <= 214 || ch >= 216 && ch <= 246 || ch >= 248 && ch <= 255) {AddCh(); goto case 1;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 2:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '.' || ch >= '0' && ch <= 'Z' || ch >= ']' && ch <= 65535) {AddCh(); goto case 2;}
				else if (ch == '/') {AddCh(); goto case 3;}
				else if (ch == 92) {AddCh(); goto case 5;}
				else if (ch == '[') {AddCh(); goto case 6;}
				else {goto case 0;}
			case 3:
				recEnd = pos; recKind = 2;
				if (ch == ',') {apx++; AddCh(); goto case 17;}
				else if (ch == 9 || ch >= 11 && ch <= 12 || ch == ' ') {apx++; AddCh(); goto case 4;}
				else if (ch == '$' || ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z' || ch == 128 || ch >= 160 && ch <= 179 || ch == 181 || ch == 186 || ch >= 192 && ch <= 214 || ch >= 216 && ch <= 246 || ch >= 248 && ch <= 255) {AddCh(); goto case 3;}
				else {t.kind = 2; break;}
			case 4:
				recEnd = pos; recKind = 2;
				if (ch == ',') {apx++; AddCh(); goto case 18;}
				else if (ch == 9 || ch >= 11 && ch <= 12 || ch == ' ') {apx++; AddCh(); goto case 4;}
				else {t.kind = 2; break;}
			case 5:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 2;}
				else {goto case 0;}
			case 6:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 7;}
				else if (ch == 92) {AddCh(); goto case 10;}
				else {goto case 0;}
			case 7:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 7;}
				else if (ch == ']') {AddCh(); goto case 8;}
				else if (ch == 92) {AddCh(); goto case 9;}
				else {goto case 0;}
			case 8:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '.' || ch >= '0' && ch <= 'Z' || ch >= ']' && ch <= 65535) {AddCh(); goto case 2;}
				else if (ch == '/') {AddCh(); goto case 3;}
				else if (ch == 92) {AddCh(); goto case 5;}
				else if (ch == '[') {AddCh(); goto case 78;}
				else {goto case 0;}
			case 9:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 7;}
				else {goto case 0;}
			case 10:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 7;}
				else {goto case 0;}
			case 11:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 12;}
				else if (ch == 92) {AddCh(); goto case 15;}
				else {goto case 0;}
			case 12:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 12;}
				else if (ch == ']') {AddCh(); goto case 13;}
				else if (ch == 92) {AddCh(); goto case 14;}
				else {goto case 0;}
			case 13:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '.' || ch >= '0' && ch <= 'Z' || ch >= ']' && ch <= 65535) {AddCh(); goto case 2;}
				else if (ch == '/') {AddCh(); goto case 3;}
				else if (ch == 92) {AddCh(); goto case 5;}
				else if (ch == '[') {AddCh(); goto case 79;}
				else {goto case 0;}
			case 14:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 12;}
				else {goto case 0;}
			case 15:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 12;}
				else {goto case 0;}
			case 16:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 2;}
				else {goto case 0;}
			case 17:
				{
					tlen -= apx;
					SetScannerBehindT();
					t.kind = 2; break;}
			case 18:
				{
					tlen -= apx;
					SetScannerBehindT();
					t.kind = 2; break;}
			case 19:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 25;}
				else if (ch == '+' || ch == '-') {AddCh(); goto case 20;}
				else {goto case 0;}
			case 20:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 25;}
				else {goto case 0;}
			case 21:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 80;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 19;}
				else {t.kind = 3; break;}
			case 22:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 82;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 19;}
				else {t.kind = 3; break;}
			case 23:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 84;}
				else {goto case 0;}
			case 24:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 24;}
				else {t.kind = 3; break;}
			case 25:
				{t.kind = 3; break;}
			case 26:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 85;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 86;}
				else {goto case 0;}
			case 27:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 28:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 'T' || ch >= 'V' && ch <= 't' || ch >= 'v' && ch <= 'w' || ch >= 'y' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == 'x') {AddCh(); goto case 87;}
				else if (ch == 'u') {AddCh(); goto case 88;}
				else if (ch == 'U') {AddCh(); goto case 89;}
				else {goto case 0;}
			case 29:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 90;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 91;}
				else {goto case 0;}
			case 30:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 31:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 'T' || ch >= 'V' && ch <= 't' || ch >= 'v' && ch <= 'w' || ch >= 'y' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 'x') {AddCh(); goto case 92;}
				else if (ch == 'u') {AddCh(); goto case 93;}
				else if (ch == 'U') {AddCh(); goto case 94;}
				else {goto case 0;}
			case 32:
				{t.kind = 4; break;}
			case 33:
				{t.kind = 34; break;}
			case 34:
				{t.kind = 35; break;}
			case 35:
				{t.kind = 41; break;}
			case 36:
				{t.kind = 42; break;}
			case 37:
				{t.kind = 43; break;}
			case 38:
				{t.kind = 45; break;}
			case 39:
				{t.kind = 46; break;}
			case 40:
				{t.kind = 47; break;}
			case 41:
				{t.kind = 48; break;}
			case 42:
				{t.kind = 49; break;}
			case 43:
				{t.kind = 50; break;}
			case 44:
				{t.kind = 51; break;}
			case 45:
				{t.kind = 55; break;}
			case 46:
				{t.kind = 60; break;}
			case 47:
				{t.kind = 62; break;}
			case 48:
				{t.kind = 63; break;}
			case 49:
				{t.kind = 64; break;}
			case 50:
				{t.kind = 65; break;}
			case 51:
				{t.kind = 68; break;}
			case 52:
				{t.kind = 70; break;}
			case 53:
				{t.kind = 72; break;}
			case 54:
				{t.kind = 73; break;}
			case 55:
				{t.kind = 74; break;}
			case 56:
				{t.kind = 76; break;}
			case 57:
				{t.kind = 77; break;}
			case 58:
				{t.kind = 78; break;}
			case 59:
				{t.kind = 79; break;}
			case 60:
				{t.kind = 80; break;}
			case 61:
				{t.kind = 81; break;}
			case 62:
				{t.kind = 82; break;}
			case 63:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 81;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 19;}
				else if (ch == '.') {AddCh(); goto case 22;}
				else {t.kind = 3; break;}
			case 64:
				recEnd = pos; recKind = 37;
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= ')' || ch >= '+' && ch <= '.' || ch >= '0' && ch <= '<' || ch >= '>' && ch <= 'Z' || ch >= ']' && ch <= 65535) {AddCh(); goto case 2;}
				else if (ch == '[') {AddCh(); goto case 11;}
				else if (ch == 92) {AddCh(); goto case 16;}
				else if (ch == '=') {AddCh(); goto case 95;}
				else {t.kind = 37; break;}
			case 65:
				recEnd = pos; recKind = 75;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 21;}
				else {t.kind = 75; break;}
			case 66:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 81;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 19;}
				else if (ch == '.') {AddCh(); goto case 22;}
				else if (ch == 'X' || ch == 'x') {AddCh(); goto case 23;}
				else {t.kind = 3; break;}
			case 67:
				recEnd = pos; recKind = 39;
				if (ch == '+') {AddCh(); goto case 33;}
				else if (ch == '=') {AddCh(); goto case 36;}
				else {t.kind = 39; break;}
			case 68:
				recEnd = pos; recKind = 40;
				if (ch == '-') {AddCh(); goto case 34;}
				else if (ch == '=') {AddCh(); goto case 35;}
				else {t.kind = 40; break;}
			case 69:
				recEnd = pos; recKind = 36;
				if (ch == '=') {AddCh(); goto case 37;}
				else {t.kind = 36; break;}
			case 70:
				recEnd = pos; recKind = 38;
				if (ch == '=') {AddCh(); goto case 38;}
				else {t.kind = 38; break;}
			case 71:
				recEnd = pos; recKind = 57;
				if (ch == '=') {AddCh(); goto case 39;}
				else if (ch == '|') {AddCh(); goto case 49;}
				else {t.kind = 57; break;}
			case 72:
				recEnd = pos; recKind = 56;
				if (ch == '=') {AddCh(); goto case 40;}
				else if (ch == '&') {AddCh(); goto case 48;}
				else {t.kind = 56; break;}
			case 73:
				recEnd = pos; recKind = 58;
				if (ch == '=') {AddCh(); goto case 41;}
				else if (ch == '^') {AddCh(); goto case 50;}
				else {t.kind = 58; break;}
			case 74:
				recEnd = pos; recKind = 61;
				if (ch == '<') {AddCh(); goto case 96;}
				else if (ch == '=') {AddCh(); goto case 47;}
				else {t.kind = 61; break;}
			case 75:
				recEnd = pos; recKind = 59;
				if (ch == '>') {AddCh(); goto case 97;}
				else if (ch == '=') {AddCh(); goto case 46;}
				else {t.kind = 59; break;}
			case 76:
				recEnd = pos; recKind = 66;
				if (ch == '=') {AddCh(); goto case 98;}
				else {t.kind = 66; break;}
			case 77:
				recEnd = pos; recKind = 71;
				if (ch == '=') {AddCh(); goto case 99;}
				else {t.kind = 71; break;}
			case 78:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 100;}
				else if (ch == 92) {AddCh(); goto case 101;}
				else {goto case 0;}
			case 79:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 102;}
				else if (ch == 92) {AddCh(); goto case 103;}
				else {goto case 0;}
			case 80:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 80;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 19;}
				else {t.kind = 3; break;}
			case 81:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 81;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 19;}
				else if (ch == '.') {AddCh(); goto case 22;}
				else {t.kind = 3; break;}
			case 82:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 83;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 19;}
				else {t.kind = 3; break;}
			case 83:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 83;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 19;}
				else {t.kind = 3; break;}
			case 84:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 24;}
				else {t.kind = 3; break;}
			case 85:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 86:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 'T' || ch >= 'V' && ch <= 't' || ch >= 'v' && ch <= 'w' || ch >= 'y' && ch <= 65535) {AddCh(); goto case 85;}
				else if (ch == 'x') {AddCh(); goto case 104;}
				else if (ch == 'u') {AddCh(); goto case 105;}
				else if (ch == 'U') {AddCh(); goto case 106;}
				else {goto case 0;}
			case 87:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 107;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 88:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 108;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 89:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 109;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 90:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 91:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 'T' || ch >= 'V' && ch <= 't' || ch >= 'v' && ch <= 'w' || ch >= 'y' && ch <= 65535) {AddCh(); goto case 90;}
				else if (ch == 'x') {AddCh(); goto case 110;}
				else if (ch == 'u') {AddCh(); goto case 111;}
				else if (ch == 'U') {AddCh(); goto case 112;}
				else {goto case 0;}
			case 92:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 113;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 93:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 114;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 94:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 115;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 95:
				recEnd = pos; recKind = 44;
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '.' || ch >= '0' && ch <= 'Z' || ch >= ']' && ch <= 65535) {AddCh(); goto case 2;}
				else if (ch == '/') {AddCh(); goto case 3;}
				else if (ch == 92) {AddCh(); goto case 5;}
				else if (ch == '[') {AddCh(); goto case 6;}
				else {t.kind = 44; break;}
			case 96:
				recEnd = pos; recKind = 53;
				if (ch == '=') {AddCh(); goto case 42;}
				else {t.kind = 53; break;}
			case 97:
				recEnd = pos; recKind = 52;
				if (ch == '=') {AddCh(); goto case 43;}
				else if (ch == '>') {AddCh(); goto case 116;}
				else {t.kind = 52; break;}
			case 98:
				recEnd = pos; recKind = 69;
				if (ch == '=') {AddCh(); goto case 52;}
				else {t.kind = 69; break;}
			case 99:
				recEnd = pos; recKind = 67;
				if (ch == '=') {AddCh(); goto case 51;}
				else {t.kind = 67; break;}
			case 100:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 100;}
				else if (ch == ']') {AddCh(); goto case 8;}
				else if (ch == 92) {AddCh(); goto case 117;}
				else {goto case 0;}
			case 101:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 100;}
				else {goto case 0;}
			case 102:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 102;}
				else if (ch == ']') {AddCh(); goto case 118;}
				else if (ch == 92) {AddCh(); goto case 119;}
				else {goto case 0;}
			case 103:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 102;}
				else {goto case 0;}
			case 104:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 120;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 105:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 121;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 106:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 122;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 107:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 108:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 123;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 109:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 124;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 110:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 125;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 111:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 126;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 112:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 127;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 113:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 114:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 128;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 115:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 129;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 116:
				recEnd = pos; recKind = 54;
				if (ch == '=') {AddCh(); goto case 44;}
				else {t.kind = 54; break;}
			case 117:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 100;}
				else {goto case 0;}
			case 118:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '.' || ch >= '0' && ch <= 'Z' || ch >= ']' && ch <= 65535) {AddCh(); goto case 2;}
				else if (ch == '/') {AddCh(); goto case 3;}
				else if (ch == 92) {AddCh(); goto case 5;}
				else if (ch == '[') {AddCh(); goto case 130;}
				else {goto case 0;}
			case 119:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 102;}
				else {goto case 0;}
			case 120:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 85;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 121:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 131;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 122:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 132;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 123:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 133;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 124:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 134;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 125:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 90;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 126:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 135;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 127:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 136;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 128:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 137;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 129:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 138;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 130:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 139;}
				else if (ch == 92) {AddCh(); goto case 140;}
				else {goto case 0;}
			case 131:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 141;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 132:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 142;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 133:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 134:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 143;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 135:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 144;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 136:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 145;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 137:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 138:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 146;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 139:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '[' || ch >= '^' && ch <= 65535) {AddCh(); goto case 139;}
				else if (ch == ']') {AddCh(); goto case 118;}
				else if (ch == 92) {AddCh(); goto case 147;}
				else {goto case 0;}
			case 140:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 139;}
				else {goto case 0;}
			case 141:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 85;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 142:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 148;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 143:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 149;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 144:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 90;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 145:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 150;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 146:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 151;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 147:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) {AddCh(); goto case 139;}
				else {goto case 0;}
			case 148:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 152;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 149:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 153;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 150:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 154;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 151:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 155;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 152:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 156;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 153:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 157;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 154:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 158;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 155:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 159;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 156:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 160;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 157:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 158:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 161;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 159:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}
			case 160:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 85;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 28;}
				else {goto case 0;}
			case 161:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 90;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) {AddCh(); goto case 30;}
				else if (ch == 39) {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 31;}
				else {goto case 0;}

		}
		t.val = new String(tval, 0, tlen);
		return t;
	}
	
	private void SetScannerBehindT() {
		buffer.Pos = t.pos;
		NextCh();
		line = t.line; col = t.col; charPos = t.charPos;
		for (int i = 0; i < tlen; i++) NextCh();
	}
	
	// get the next token (possibly a token already seen during peeking)
	public Token Scan () {
		if (tokens.next == null) {
			return NextToken();
		} else {
			pt = tokens = tokens.next;
			return tokens;
		}
	}

	// peek for the next token, ignore pragmas
	public Token Peek () {
		do {
			if (pt.next == null) {
				pt.next = NextToken();
			}
			pt = pt.next;
		} while (pt.kind > maxT); // skip pragmas
	
		return pt;
	}

	// make sure that peeking starts at the current scan position
	public void ResetPeek () { pt = tokens; }

} // end Scanner
}