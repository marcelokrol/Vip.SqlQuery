﻿using Vip.SqlQuery.Clause;
using Vip.SqlQuery.Enums;

namespace Vip.SqlQuery
{
    public partial class SqlQuery
    {
        #region Where

        public SqlQuery Where(string column, Condition condition, object value)
        {
            _whereList.Add(new WhereClause(column, condition, value, LogicOperator.NULL, parameterNumber));
            parameterNumber++;
            return this;
        }

        public SqlQuery Where(Where[] multiple)
        {
            _whereList.Add(new WhereClause(multiple, LogicOperator.NULL, parameterNumber));
            parameterNumber += multiple.Length;
            return this;
        }

        #endregion

        #region WhereAnd

        public SqlQuery WhereAnd(string column, Condition condition, object value)
        {
            _whereList.Add(new WhereClause(column, condition, value, LogicOperator.AND, parameterNumber));
            parameterNumber++;
            return this;
        }

        public SqlQuery WhereAnd(Where[] multiple)
        {
            _whereList.Add(new WhereClause(multiple, LogicOperator.AND, parameterNumber));
            parameterNumber += multiple.Length;
            return this;
        }

        #endregion

        #region WhereOr

        public SqlQuery WhereOr(string column, Condition condition, object value)
        {
            _whereList.Add(new WhereClause(column, condition, value, LogicOperator.OR, parameterNumber));
            parameterNumber++;
            return this;
        }

        public SqlQuery WhereOr(Where[] multiple)
        {
            _whereList.Add(new WhereClause(multiple, LogicOperator.OR, parameterNumber));
            parameterNumber += multiple.Length;
            return this;
        }

        #endregion
    }
}