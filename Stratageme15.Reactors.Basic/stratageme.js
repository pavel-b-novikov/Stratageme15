Object.prototype.__getFullQualifiedName = Object.prototype.__getFullQualifiedName || (function () {
    var funcNameRegex = /function (.{1,})\(/;
    var results = (funcNameRegex).exec((this).constructor.toString());
    return (results && results.length > 1) ? results[1] : "";
});

var __extends = this.__extends || (function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
});

var __namespace = this.__namespace || (function (ns, c) {
    if (!ns || ns.length == 0) return;
    var nsPath = ns.split('.');
    var co = this;
    for (var i = 0; i < nsPath.length; i++) {
        var p = nsPath[i];
        if (!co[p]) co[p] = {};
        co = co[p];
    }
    co[c.name] = c;
    c.getNamespace = function () { return ns; };
    var fullTypeName = ns + "." + c.name;
    c.getFullQualifiedName = function () { return fullTypeName; };
});

var __matchArguments = this.__matchArguments || (function() {
    var aa = arguments[0];
    if (aa.length != (arguments.length + 1)) return false;
    for (var i = 0; i < aa.length; i++) {
        var o = aa[i];
        var ts = arguments[i + 1];
        if (o.__getFullQualifiedName() != ts) return false;
    }
    return true;
});