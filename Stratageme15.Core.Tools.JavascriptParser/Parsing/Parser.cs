
using System;
using System.Collections;
using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Tools.JavascriptParser;
using Stratageme15.Core.Tools.ParsingErrors;
namespace Stratageme15.Core.Tools.JavascriptParser {



public partial class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _regexL = 2;
	public const int _numberL = 3;
	public const int _stringL = 4;
	public const int _break = 5;
	public const int _case = 6;
	public const int _catch = 7;
	public const int _continue = 8;
	public const int _debugger = 9;
	public const int _default = 10;
	public const int _delete = 11;
	public const int _do = 12;
	public const int _else = 13;
	public const int _finally = 14;
	public const int _for = 15;
	public const int _function = 16;
	public const int _if = 17;
	public const int _in = 18;
	public const int _instanceof = 19;
	public const int _new = 20;
	public const int _of = 21;
	public const int _return = 22;
	public const int _switch = 23;
	public const int _this = 24;
	public const int _throw = 25;
	public const int _try = 26;
	public const int _typeof = 27;
	public const int _trueW = 28;
	public const int _falseW = 29;
	public const int _var = 30;
	public const int _void = 31;
	public const int _while = 32;
	public const int _with = 33;
	public const int _inc = 34;
	public const int _dec = 35;
	public const int _times = 36;
	public const int _div = 37;
	public const int _mod = 38;
	public const int _plus = 39;
	public const int _minus = 40;
	public const int _aminus = 41;
	public const int _aplus = 42;
	public const int _atimes = 43;
	public const int _adiv = 44;
	public const int _amod = 45;
	public const int _abor = 46;
	public const int _aband = 47;
	public const int _abxor = 48;
	public const int _alsh = 49;
	public const int _arsh = 50;
	public const int _arush = 51;
	public const int _rshift = 52;
	public const int _lshift = 53;
	public const int _urshift = 54;
	public const int _bnot = 55;
	public const int _band = 56;
	public const int _bor = 57;
	public const int _bxor = 58;
	public const int _gt = 59;
	public const int _gteq = 60;
	public const int _lt = 61;
	public const int _lteq = 62;
	public const int _and = 63;
	public const int _or = 64;
	public const int _xor = 65;
	public const int _not = 66;
	public const int _eq = 67;
	public const int _seq = 68;
	public const int _neq = 69;
	public const int _nseq = 70;
	public const int _assgn = 71;
	public const int _colon = 72;
	public const int _scolon = 73;
	public const int _comma = 74;
	public const int _dot = 75;
	public const int _question = 76;
	public const int _lbrace = 77;
	public const int _lbrack = 78;
	public const int _lpar = 79;
	public const int _rbrace = 80;
	public const int _rbrack = 81;
	public const int _rpar = 82;
	public const int maxT = 85;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	private Token t;    // last recognized token
	private Token la;   // lookahead token
	int errDist = minErrDist;



	
	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(t.line, t.col, n,t.val);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}
	Token Peek (int n) {
	  scanner.ResetPeek();
	  Token _x = la;
	  while (n > 0) {
		_x = scanner.Peek();
		n--;
	  }
	  return _x;
	}

	
	void JS() {
		RootStatements();
		while (StartOf(1)) {
			RootStatements();
		}
		EatColon(); 
	}

	void RootStatements() {
		EatLabel(); 
		if (la.kind == 73) {
			Get();
			Terminal<EmptyStatement>(); 
		} else if (la.kind == 30) {
			VariableDef();
			EatColon(); 
		} else if (la.kind == 17) {
			IfStm();
		} else if (la.kind == 32) {
			WhileStm();
		} else if (la.kind == 12) {
			DoWhileStm();
		} else if (la.kind == 15) {
			ForStm();
		} else if (la.kind == 26) {
			TryCatchFinallyStm();
		} else if (la.kind == 23) {
			SwitchStm();
		} else if (la.kind==_lbrace) {
			if (IsObjectDef()) {
				Expression();
				EatColon(); 
			} else if (la.kind == 77) {
				Block();
				EatColon(); 
			} else SynErr(86);
		} else if (la.kind == 9) {
			Get();
			EatColon(); Terminal<DebuggerStatement>(); 
		} else if (StartOf(2)) {
			Expression();
			EatColon(); 
		} else SynErr(87);
	}

	void ParenthesisExpression() {
		Expect(79);
		Push<ParenthesisExpression>(); 
		Expression();
		Expect(82);
		Pop(); 
	}

	void Expression() {
		Push<UnaryExpression>(); 
		Unary();
		if (IsAssignment()) {
			AssignmentOperator();
			Clarify<AssignmentBinaryExpression>(); 
			NonSequenceExpression();
		} else if (StartOf(3)) {
			FixUpRegexLookahead(); 
			OrExpr();
			if (la.kind == 76) {
				TernaryPartStm();
			}
		} else SynErr(88);
		if (la.kind == 74) {
			Get();
			Converge<SequenceExpression>(); 
			NonSequenceExpression();
			while (la.kind == 74) {
				Get();
				NonSequenceExpression();
			}
		}
		NeedScolon(); Pop(); 
	}

	void FormalParameterList() {
		Push<FormalParametersList>(); 
		Expect(1);
		Ident(); 
		while (la.kind == 74) {
			Get();
			Expect(1);
			Ident(); 
		}
		Pop(); 
	}

	void VariableDef() {
		Push<VariableDefStatement>(); 
		Expect(30);
		VarList();
		Pop(); 
	}

	void VarList() {
		var nn = Peek(1);
		if (nn.kind==_comma) {
			Expect(1);
			Ident(); 
			Expect(74);
			VarList();
		} else if (!assgnOps[nn.kind]) {
			Push<AssignmentStatement>(); 
			Expect(1);
			Ident(); 
			Pop(); 
		} else if (StartOf(4)) {
			AssignStm();
			while (la.kind == 74) {
				Get();
				AssignStm();
			}
		} else SynErr(89);
	}

	void AssignStm() {
		Push<AssignmentStatement>(); 
		Push<UnaryExpression>(); 
		Primary();
		Pop(); 
		if (StartOf(5)) {
			AssignmentOperator();
			NonSequenceExpression();
		}
		Pop(); 
	}

	void ObjectDef() {
		Push<ObjectDefinitionExpression>(); 
		Expect(77);
		if (la.kind == 80) {
			Get();
		} else if (StartOf(6)) {
			ObjectFieldDef();
			while (la.kind == 74) {
				Get();
				if (StartOf(6)) {
					ObjectFieldDef();
				}
			}
			Expect(80);
		} else SynErr(90);
		Pop(); 
	}

	void ObjectFieldDef() {
		if (StartOf(7)) {
			Literal();
		} else if (StartOf(8)) {
			KeywordOrIdent();
		} else SynErr(91);
		Expect(72);
		NonSequenceExpression();
	}

	void KeywordOrIdent() {
		switch (la.kind) {
		case 5: {
			Get();
			break;
		}
		case 6: {
			Get();
			break;
		}
		case 7: {
			Get();
			break;
		}
		case 8: {
			Get();
			break;
		}
		case 9: {
			Get();
			break;
		}
		case 10: {
			Get();
			break;
		}
		case 11: {
			Get();
			break;
		}
		case 12: {
			Get();
			break;
		}
		case 13: {
			Get();
			break;
		}
		case 14: {
			Get();
			break;
		}
		case 15: {
			Get();
			break;
		}
		case 16: {
			Get();
			break;
		}
		case 17: {
			Get();
			break;
		}
		case 18: {
			Get();
			break;
		}
		case 19: {
			Get();
			break;
		}
		case 20: {
			Get();
			break;
		}
		case 21: {
			Get();
			break;
		}
		case 22: {
			Get();
			break;
		}
		case 23: {
			Get();
			break;
		}
		case 24: {
			Get();
			break;
		}
		case 25: {
			Get();
			break;
		}
		case 26: {
			Get();
			break;
		}
		case 27: {
			Get();
			break;
		}
		case 30: {
			Get();
			break;
		}
		case 31: {
			Get();
			break;
		}
		case 32: {
			Get();
			break;
		}
		case 33: {
			Get();
			break;
		}
		case 1: {
			Get();
			break;
		}
		default: SynErr(92); break;
		}
		Ident(); 
	}

	void Literal() {
		if (la.kind == 4) {
			Get();
			TerminalArg<StringLiteral>(); 
		} else if (la.kind == 3) {
			Get();
			TerminalArg<NumberLiteral>(); 
		} else if (la.kind == 28) {
			Get();
			Terminal<TrueBooleanKeywordLiteralExpression>(); 
		} else if (la.kind == 29) {
			Get();
			Terminal<FalseBooleanKeywordLiteralExpression>(); 
		} else SynErr(93);
	}

	void NonSequenceExpression() {
		Push<UnaryExpression>(); 
		Unary();
		if (IsAssignment()) {
			AssignmentOperator();
			Clarify<AssignmentBinaryExpression>(); 
			NonSequenceExpression();
		} else if (StartOf(3)) {
			OrExpr();
			if (la.kind == 76) {
				TernaryPartStm();
			}
		} else SynErr(94);
		NeedScolon(); Pop(); 
	}

	void FactParameterList() {
		Push<FactParameterList>(); 
		NonSequenceExpression();
		while (la.kind == 74) {
			Get();
			NonSequenceExpression();
		}
		Pop(); 
	}

	void Unary() {
		Push<UnaryExpression>(); 
		if (StartOf(9)) {
			switch (la.kind) {
			case 39: {
				Get();
				UnaryOp(); 
				Unary();
				break;
			}
			case 40: {
				Get();
				UnaryOp(); 
				Unary();
				break;
			}
			case 66: {
				Get();
				UnaryOp(); 
				Unary();
				break;
			}
			case 55: {
				Get();
				UnaryOp(); 
				Unary();
				break;
			}
			case 34: {
				Get();
				Clarify<PrefixIncrementDecrementExpression>(); IncDec(); 
				Unary();
				break;
			}
			case 35: {
				Get();
				Clarify<PrefixIncrementDecrementExpression>(); IncDec(); 
				Unary();
				break;
			}
			}
		} else if (StartOf(4)) {
			Primary();
		} else SynErr(95);
		Pop(); 
	}

	void AssignmentOperator() {
		FixUpRegexLookahead(); 
		switch (la.kind) {
		case 71: {
			Get();
			break;
		}
		case 42: {
			Get();
			break;
		}
		case 41: {
			Get();
			break;
		}
		case 43: {
			Get();
			break;
		}
		case 44: {
			Get();
			break;
		}
		case 49: {
			Get();
			break;
		}
		case 50: {
			Get();
			break;
		}
		case 45: {
			Get();
			break;
		}
		case 51: {
			Get();
			break;
		}
		case 46: {
			Get();
			break;
		}
		case 47: {
			Get();
			break;
		}
		case 48: {
			Get();
			break;
		}
		default: SynErr(96); break;
		}
		Assignment(); 
	}

	void OrExpr() {
		AndExpr();
		while (la.kind == 64) {
			Get();
			Converge<LogicalBinaryExpression>(); Logical();  
			Push<UnaryExpression>(); 
			Unary();
			AndExpr();
			PopDrop(); 
		}
	}

	void TernaryPartStm() {
		Expect(76);
		Converge<TernaryStatement>(); 
		OnlyParenthesisIfSequence();
		Expect(72);
		OnlyParenthesisIfSequence();
	}

	void AndExpr() {
		SystemExpr();
		while (la.kind == 63) {
			Get();
			Converge<LogicalBinaryExpression>(); Logical();  
			Push<UnaryExpression>(); 
			Unary();
			SystemExpr();
			PopDrop(); 
		}
	}

	void SystemExpr() {
		BitOrExpr();
		while (la.kind == 18 || la.kind == 19) {
			if (la.kind == 18) {
				Get();
			} else {
				Get();
			}
			Converge<LogicalBinaryExpression>(); Logical();  
			Push<UnaryExpression>(); 
			Unary();
			BitOrExpr();
			PopDrop(); 
		}
	}

	void BitOrExpr() {
		BitXorExpr();
		while (la.kind == 57) {
			Get();
			Converge<BitwiseBinaryExpression>(); Bitwise(); 
			Push<UnaryExpression>(); 
			Unary();
			BitXorExpr();
			PopDrop(); 
		}
	}

	void BitXorExpr() {
		BitAndExpr();
		while (la.kind == 58) {
			Get();
			Converge<BitwiseBinaryExpression>(); Bitwise(); 
			Push<UnaryExpression>(); 
			Unary();
			BitAndExpr();
			PopDrop(); 
		}
	}

	void BitAndExpr() {
		EqlExpr();
		while (la.kind == 56) {
			Get();
			Converge<BitwiseBinaryExpression>(); Bitwise(); 
			Push<UnaryExpression>(); 
			Unary();
			EqlExpr();
			PopDrop(); 
		}
	}

	void EqlExpr() {
		RelExpr();
		while (StartOf(10)) {
			if (la.kind == 69) {
				Get();
			} else if (la.kind == 67) {
				Get();
			} else if (la.kind == 68) {
				Get();
			} else {
				Get();
			}
			Converge<ComparisonBinaryExpression>();  Comparison(); 
			Push<UnaryExpression>(); 
			Unary();
			RelExpr();
			PopDrop(); 
		}
	}

	void RelExpr() {
		ShiftExpr();
		while (StartOf(11)) {
			if (la.kind == 61) {
				Get();
			} else if (la.kind == 59) {
				Get();
			} else if (la.kind == 62) {
				Get();
			} else {
				Get();
			}
			Converge<ComparisonBinaryExpression>();  Comparison(); 
			Push<UnaryExpression>(); 
			Unary();
			ShiftExpr();
			PopDrop(); 
		}
	}

	void ShiftExpr() {
		AddExpr();
		while (la.kind == 52 || la.kind == 53 || la.kind == 54) {
			if (la.kind == 53) {
				Get();
			} else if (la.kind == 52) {
				Get();
			} else {
				Get();
			}
			Converge<BitwiseBinaryExpression>(); Bitwise();  
			Push<UnaryExpression>(); 
			Unary();
			AddExpr();
			PopDrop(); 
		}
	}

	void AddExpr() {
		MulExpr();
		while (la.kind == 39 || la.kind == 40) {
			if (la.kind == 39) {
				Get();
			} else {
				Get();
			}
			Converge<MathBinaryExpression>(); Math(); 
			Push<UnaryExpression>(); 
			Unary();
			MulExpr();
			PopDrop(); 
		}
	}

	void MulExpr() {
		FixUpRegexLookahead(); 
		while (la.kind == 36 || la.kind == 37 || la.kind == 38) {
			if (la.kind == 36) {
				Get();
			} else if (la.kind == 37) {
				Get();
			} else {
				Get();
			}
			FixUpRegexLookahead(); 
			Converge<MathBinaryExpression>(); Math(); 
			Unary();
			FixUpRegexLookahead(); 
		}
	}

	void Primary() {
		switch (la.kind) {
		case 2: case 3: case 4: case 28: case 29: {
			RegexOrLiteral();
			break;
		}
		case 77: {
			ObjectDef();
			break;
		}
		case 16: {
			FunctionDef();
			break;
		}
		case 79: {
			Get();
			Push<ParenthesisExpression>(); 
			Expression();
			Pop(); 
			Expect(82);
			break;
		}
		case 78: {
			ArrayDef();
			break;
		}
		case 24: {
			Get();
			Terminal<ThisKeywordLiteralExpression>(); 
			break;
		}
		case 83: {
			Get();
			Terminal<NullKeywordLiteralExpression>(); 
			break;
		}
		case 11: {
			DeleteStm();
			break;
		}
		case 31: {
			VoidStm();
			break;
		}
		case 20: {
			Get();
			Push<NewInvokationExpression>();  
			if (StartOf(8)) {
				FieldAccessExpression();
			}
			if (la.kind == 79) {
				Get();
				if (StartOf(2)) {
					FactParameterList();
				}
				Expect(82);
			}
			Pop(); 
			break;
		}
		case 27: {
			Get();
			Push<TypeofExpression>(); 
			Primary();
			Pop(); 
			break;
		}
		case 1: {
			Get();
			Ident(); 
			break;
		}
		default: SynErr(97); break;
		}
		while (StartOf(12)) {
			ExpressionPostfix();
		}
	}

	void FunctionDef() {
		Push<FunctionDefExpression>(); 
		Expect(16);
		if (la.kind == 1) {
			Get();
			Ident(); 
		}
		Expect(79);
		if (la.kind == 1) {
			FormalParameterList();
		}
		Expect(82);
		Block();
		NeedScolon(); Pop(); 
	}

	void Block() {
		Push<CodeBlock>(); 
		Expect(77);
		while (StartOf(13)) {
			Statement();
		}
		Expect(80);
		Pop(); 
	}

	void ArrayDef() {
		Push<ArrayCreationExpression>(); 
		Expect(78);
		if (StartOf(2)) {
			OnlyParenthesisIfSequence();
			while (la.kind == 74) {
				Get();
				OnlyParenthesisIfSequence();
			}
		}
		Expect(81);
		Pop(); 
	}

	void OnlyParenthesisIfSequence() {
		if (IsParenthesedSequence()) {
			Expect(79);
			Push<ParenthesisExpression>(); Push<SequenceExpression>(); 
			NonSequenceExpression();
			while (la.kind == 74) {
				Get();
				NonSequenceExpression();
			}
			Pop(); Pop(); 
			Expect(82);
		} else if (StartOf(2)) {
			NonSequenceExpression();
		} else SynErr(98);
	}

	void FieldAccessExpression() {
		Push<FieldAccessExpression>(); 
		KeywordOrIdent();
		while (la.kind == 75) {
			Get();
			Converge<FieldAccessExpression>(); 
			KeywordOrIdent();
		}
		Pop(); 
	}

	void RegexOrLiteral() {
		if (la.kind == 2) {
			Get();
			TerminalArg<RegexLiteral>(); 
		} else if (StartOf(7)) {
			Literal();
		} else SynErr(99);
	}

	void DeleteStm() {
		Push<DeleteStatement>(); 
		Expect(11);
		Expression();
		EatColon(); 
		Pop(); 
	}

	void VoidStm() {
		Push<VoidExpression>(); 
		Expect(31);
		OnlyParenthesisIfSequence();
		Pop(); 
	}

	void ExpressionPostfix() {
		if (la.kind == 34) {
			Get();
			Converge<PostfixIncrementDecrementExpression>(); IncDec(); 
		} else if (la.kind == 35) {
			Get();
			Converge<PostfixIncrementDecrementExpression>(); IncDec(); 
		} else if (la.kind == 78) {
			Get();
			Converge<IndexerExpression>(); Push<IndexExpression>(); 
			OnlyParenthesisIfSequence();
			Pop(); 
			Expect(81);
		} else if (la.kind == 75) {
			Get();
			Converge<FieldAccessExpression>(); 
			KeywordOrIdent();
			while (la.kind == 75) {
				Get();
				Converge<FieldAccessExpression>(); 
				KeywordOrIdent();
			}
		} else if (la.kind == 79) {
			Get();
			Converge<CallExpression>(); 
			if (StartOf(2)) {
				FactParameterList();
			}
			Expect(82);
		} else SynErr(100);
	}

	void IfStm() {
		Push<IfStatement>(); 
		Expect(17);
		ParenthesisExpression();
		if (la.kind == 77) {
			Block();
		} else if (StartOf(13)) {
			Statement();
			EatColon(); 
		} else SynErr(101);
		while (IsElseIf()) {
			Expect(13);
			Expect(17);
			ParenthesisExpression();
			if (la.kind == 77) {
				Block();
			} else if (StartOf(13)) {
				Statement();
				EatColon(); 
			} else SynErr(102);
		}
		if (la.kind == 13) {
			Get();
			if (la.kind == 77) {
				Block();
			} else if (StartOf(13)) {
				Statement();
				EatColon(); 
			} else SynErr(103);
		}
		EatColon(); 
		Pop(); 
	}

	void WhileStm() {
		Push<WhileStatement>(); 
		Expect(32);
		ParenthesisExpression();
		if (la.kind == 73) {
			Get();
		} else if (StartOf(13)) {
			if (la.kind == 77) {
				Block();
			} else {
				Statement();
			}
			EatColon(); 
		} else SynErr(104);
		Pop(); 
	}

	void DoWhileStm() {
		Push<DoWhileStatement>(); 
		Expect(12);
		if (la.kind == 77) {
			Block();
		} else if (StartOf(13)) {
			Statement();
		} else SynErr(105);
		Expect(32);
		ParenthesisExpression();
		EatColon(); 
		Pop(); 
	}

	void ForStm() {
		Expect(15);
		Expect(79);
		if (IsForin()) {
			Push<ForInStatement>(); Push<ForInStatementVariableDeclaration>(); 
			if (la.kind == 30 || la.kind == 84) {
				if (la.kind == 30) {
					Get();
				} else {
					Get();
				}
			}
			TerminalArg<VarLetModifier>(); 
			Expect(1);
			Ident(); 
			Pop(); 
			Expect(18);
			Expression();
			Expect(82);
		} else if (IsForof()) {
			Push<ForOfStatement>(); Push<ForInStatementVariableDeclaration>(); 
			if (la.kind == 30 || la.kind == 84) {
				if (la.kind == 30) {
					Get();
				} else {
					Get();
				}
			}
			TerminalArg<VarLetModifier>(); 
			Expect(1);
			Ident(); 
			Pop(); 
			Expect(21);
			Expression();
			Expect(82);
		} else if (StartOf(14)) {
			Push<ForStatement>(); 
			if (StartOf(15)) {
				if (la.kind == 30) {
					VariableDef();
				} else {
					Expression();
				}
			}
			Expect(73);
			if (StartOf(2)) {
				Expression();
			}
			Expect(73);
			if (StartOf(2)) {
				Expression();
			}
			Expect(82);
		} else SynErr(106);
		if (la.kind == 73) {
			Get();
		} else if (StartOf(13)) {
			if (la.kind == 77) {
				Block();
			} else {
				Statement();
			}
			EatColon(); 
		} else SynErr(107);
		Pop(); 
	}

	void TryCatchFinallyStm() {
		Push<TryCatchFinallyStatement>(); 
		Expect(26);
		Block();
		if (la.kind == 7) {
			Get();
			Push<CatchClause>(); 
			Expect(79);
			if (la.kind == 1) {
				Get();
				Ident(); 
			}
			Expect(82);
			Block();
			Pop(); 
		}
		if (la.kind == 14) {
			Get();
			Push<FinallyClause>(); 
			Block();
			Pop(); 
		}
		Pop(); 
	}

	void SwitchStm() {
		Push<SwitchStatement>(); 
		Expect(23);
		ParenthesisExpression();
		Expect(77);
		Expect(6);
		Push<CaseClause>(); 
		Expression();
		Expect(72);
		if (StartOf(13)) {
			if (la.kind == 77) {
				Block();
			} else {
				Statement();
				while (StartOf(13)) {
					Statement();
				}
			}
		}
		Pop(); 
		while (la.kind == 6) {
			Get();
			Push<CaseClause>(); 
			Expression();
			Expect(72);
			if (StartOf(13)) {
				if (la.kind == 77) {
					Block();
				} else {
					Statement();
					while (StartOf(13)) {
						Statement();
					}
				}
			}
			Pop(); 
		}
		if (la.kind == 10) {
			Get();
			Push<DefaultClause>(); 
			Expect(72);
			if (StartOf(13)) {
				if (la.kind == 77) {
					Block();
				} else {
					Statement();
					while (StartOf(13)) {
						Statement();
					}
				}
			}
			Pop(); 
		}
		while (la.kind == 6) {
			Get();
			Push<CaseClause>(); 
			Expression();
			Expect(72);
			if (StartOf(13)) {
				if (la.kind == 77) {
					Block();
				} else {
					Statement();
					while (StartOf(13)) {
						Statement();
					}
				}
			}
			Pop(); 
		}
		Expect(80);
		EatColon(); 
		Pop(); 
	}

	void Statement() {
		EatLabel(); 
		if (la.kind == 73) {
			Get();
			Terminal<EmptyStatement>(); 
		} else if (la.kind == 30) {
			VariableDef();
			EatColon(); 
		} else if (la.kind == 22) {
			ReturnStatement();
		} else if (la.kind == 17) {
			IfStm();
		} else if (la.kind == 32) {
			WhileStm();
		} else if (la.kind == 12) {
			DoWhileStm();
		} else if (la.kind == 5) {
			BreakStm();
		} else if (la.kind == 8) {
			ContinueStm();
		} else if (la.kind == 15) {
			ForStm();
		} else if (la.kind == 25) {
			ThrowStm();
		} else if (la.kind == 26) {
			TryCatchFinallyStm();
		} else if (la.kind == 23) {
			SwitchStm();
		} else if (la.kind==_lbrace) {
			if (IsObjectDef()) {
				Expression();
				EatColon(); 
			} else if (la.kind == 77) {
				Block();
				EatColon(); 
			} else SynErr(108);
		} else if (la.kind == 9) {
			Get();
			EatColon(); Terminal<DebuggerStatement>(); 
		} else if (StartOf(2)) {
			Expression();
			EatColon(); 
		} else SynErr(109);
	}

	void ReturnStatement() {
		Push<ReturnStatement>(); 
		Expect(22);
		if (StartOf(2)) {
			Expression();
		}
		EatColon(); 
		Pop(); 
	}

	void BreakStm() {
		Push<BreakStatement>(); 
		Expect(5);
		if (la.kind == 1) {
			Get();
			Ident(); 
		}
		EatColon(); 
		Pop(); 
	}

	void ContinueStm() {
		Push<ContinueStatement>(); 
		Expect(8);
		if (la.kind == 1) {
			Get();
			Ident(); 
		}
		EatColon(); 
		Pop(); 
	}

	void ThrowStm() {
		Push<ThrowStatement>(); 
		Expect(25);
		Expression();
		EatColon(); 
		Pop(); 
	}



	public JsProgram Parse() {
		la = new Token();
		la.val = "";		
		Get();
		JS();
		Expect(0);

		errors.ThrowIfErrors(Tree);
		return NodeTreeCompiler.Compile(Tree.Root);
	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,x,x,x, x,T,x,T, T,x,x,T, T,T,x,x, T,x,x,T, T,x,T,T, T,T,T,T, T,x,T,T, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,T,x,x, x,T,T,T, x,x,x,T, x,x,x},
		{x,T,T,T, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, T,x,x,T, T,T,x,T, x,x,T,T, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,T,T,T, x,x,x,T, x,x,x},
		{T,T,T,T, T,T,T,x, T,T,T,T, T,T,x,T, T,T,T,T, T,x,T,T, T,T,T,T, T,T,T,T, T,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,x,x},
		{x,T,T,T, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, T,x,x,T, T,T,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,T, x,x,x,T, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,T, T,T,T,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,x,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,T, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, T,x,x,T, T,T,x,x, T,x,T,T, T,T,T,T, T,T,T,T, T,x,T,T, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,T,x,x, x,T,T,T, x,x,x,T, x,x,x},
		{x,T,T,T, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, T,x,x,T, T,T,T,T, x,x,T,T, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,T,x,x, x,T,T,T, x,x,x,T, x,x,x},
		{x,T,T,T, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, T,x,x,T, T,T,T,T, x,x,T,T, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,T,T,T, x,x,x,T, x,x,x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text
	public bool HasErrors {get; private set;}
	public bool HasWarnings {get;private set;}
	public List<ParseringIssueDescription> Issues {get; private set;}

	public Errors(){
		HasErrors = false;
		HasWarnings = false;

		Issues = new List<ParseringIssueDescription>();
	}

	public virtual void SynErr (int line, int col, int n,string got) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "regexL expected"; break;
			case 3: s = "numberL expected"; break;
			case 4: s = "stringL expected"; break;
			case 5: s = "break expected"; break;
			case 6: s = "case expected"; break;
			case 7: s = "catch expected"; break;
			case 8: s = "continue expected"; break;
			case 9: s = "debugger expected"; break;
			case 10: s = "default expected"; break;
			case 11: s = "delete expected"; break;
			case 12: s = "do expected"; break;
			case 13: s = "else expected"; break;
			case 14: s = "finally expected"; break;
			case 15: s = "for expected"; break;
			case 16: s = "function expected"; break;
			case 17: s = "if expected"; break;
			case 18: s = "in expected"; break;
			case 19: s = "instanceof expected"; break;
			case 20: s = "new expected"; break;
			case 21: s = "of expected"; break;
			case 22: s = "return expected"; break;
			case 23: s = "switch expected"; break;
			case 24: s = "this expected"; break;
			case 25: s = "throw expected"; break;
			case 26: s = "try expected"; break;
			case 27: s = "typeof expected"; break;
			case 28: s = "trueW expected"; break;
			case 29: s = "falseW expected"; break;
			case 30: s = "var expected"; break;
			case 31: s = "void expected"; break;
			case 32: s = "while expected"; break;
			case 33: s = "with expected"; break;
			case 34: s = "inc expected"; break;
			case 35: s = "dec expected"; break;
			case 36: s = "times expected"; break;
			case 37: s = "div expected"; break;
			case 38: s = "mod expected"; break;
			case 39: s = "plus expected"; break;
			case 40: s = "minus expected"; break;
			case 41: s = "aminus expected"; break;
			case 42: s = "aplus expected"; break;
			case 43: s = "atimes expected"; break;
			case 44: s = "adiv expected"; break;
			case 45: s = "amod expected"; break;
			case 46: s = "abor expected"; break;
			case 47: s = "aband expected"; break;
			case 48: s = "abxor expected"; break;
			case 49: s = "alsh expected"; break;
			case 50: s = "arsh expected"; break;
			case 51: s = "arush expected"; break;
			case 52: s = "rshift expected"; break;
			case 53: s = "lshift expected"; break;
			case 54: s = "urshift expected"; break;
			case 55: s = "bnot expected"; break;
			case 56: s = "band expected"; break;
			case 57: s = "bor expected"; break;
			case 58: s = "bxor expected"; break;
			case 59: s = "gt expected"; break;
			case 60: s = "gteq expected"; break;
			case 61: s = "lt expected"; break;
			case 62: s = "lteq expected"; break;
			case 63: s = "and expected"; break;
			case 64: s = "or expected"; break;
			case 65: s = "xor expected"; break;
			case 66: s = "not expected"; break;
			case 67: s = "eq expected"; break;
			case 68: s = "seq expected"; break;
			case 69: s = "neq expected"; break;
			case 70: s = "nseq expected"; break;
			case 71: s = "assgn expected"; break;
			case 72: s = "colon expected"; break;
			case 73: s = "scolon expected"; break;
			case 74: s = "comma expected"; break;
			case 75: s = "dot expected"; break;
			case 76: s = "question expected"; break;
			case 77: s = "lbrace expected"; break;
			case 78: s = "lbrack expected"; break;
			case 79: s = "lpar expected"; break;
			case 80: s = "rbrace expected"; break;
			case 81: s = "rbrack expected"; break;
			case 82: s = "rpar expected"; break;
			case 83: s = "\"null\" expected"; break;
			case 84: s = "\"let\" expected"; break;
			case 85: s = "??? expected"; break;
			case 86: s = "invalid RootStatements"; break;
			case 87: s = "invalid RootStatements"; break;
			case 88: s = "invalid Expression"; break;
			case 89: s = "invalid VarList"; break;
			case 90: s = "invalid ObjectDef"; break;
			case 91: s = "invalid ObjectFieldDef"; break;
			case 92: s = "invalid KeywordOrIdent"; break;
			case 93: s = "invalid Literal"; break;
			case 94: s = "invalid NonSequenceExpression"; break;
			case 95: s = "invalid Unary"; break;
			case 96: s = "invalid AssignmentOperator"; break;
			case 97: s = "invalid Primary"; break;
			case 98: s = "invalid OnlyParenthesisIfSequence"; break;
			case 99: s = "invalid RegexOrLiteral"; break;
			case 100: s = "invalid ExpressionPostfix"; break;
			case 101: s = "invalid IfStm"; break;
			case 102: s = "invalid IfStm"; break;
			case 103: s = "invalid IfStm"; break;
			case 104: s = "invalid WhileStm"; break;
			case 105: s = "invalid DoWhileStm"; break;
			case 106: s = "invalid ForStm"; break;
			case 107: s = "invalid ForStm"; break;
			case 108: s = "invalid Statement"; break;
			case 109: s = "invalid Statement"; break;

			default: s = "error " + n; break;
		}
		var error = new ParseringIssueDescription(line,col,got,s);
		Issues.Add(error);
	}

	public virtual void SemErr (int line, int col, string s) {
		var error = new ParseringIssueDescription(line,col,s);
		Issues.Add(error);
	}
	
	public virtual void SemErr (string s) {
		var error = new ParseringIssueDescription(s);
		Issues.Add(error);
	}
	
	public virtual void Warning (int line, int col, string s) {
		var wrn = new ParseringIssueDescription(line,col,s,ErrorType.Warning);
		Issues.Add(wrn);
	}
	
	public virtual void Warning(string s) {
		var wrn = new ParseringIssueDescription(s,ErrorType.Warning);
		Issues.Add(wrn);
	}

	public void ThrowIfErrors(NodeInfoTree treePart){
		if (Issues.Count > 0) throw new ParseringException(Issues,treePart);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}