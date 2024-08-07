﻿using Nexo.Values;

namespace Nexo.AST
{
    public sealed class DeclExpr(string name, Expr expr, bool isConstant) : Expr
    {
        private readonly string _name = name;
        private readonly Expr _expr = expr;
        private readonly bool _isConstant = isConstant;

        public override Value Eval(Scope scope)
        {
            scope.Declare(_name, _expr.Eval(scope), _isConstant);
            return new VoidValue();
        }
    }
}
