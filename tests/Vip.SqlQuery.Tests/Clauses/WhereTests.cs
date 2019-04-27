﻿using Vip.SqlQuery.Clause;
using Vip.SqlQuery.Enums;
using Xunit;

namespace Vip.SqlQuery.Tests.Clauses
{
    public class WhereTests
    {
        private readonly string[] columnsTest = {"ProdutoId", "Descricao"};

        [Fact]
        public void WhereSimpleTest()
        {
            // Arrange
            const string queryExpected = "SELECT [ProdutoId], [Descricao] " +
                                         "FROM [Produto] " +
                                         "WHERE [ProdutoId] = @p0";

            // Act
            var query = SqlQuery.New()
                .Select(columnsTest)
                .From("Produto")
                .Where("ProdutoId", Condition.Equal, 1)
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
            Assert.Single(query.Parameters);
        }

        [Fact]
        public void WhereWithPrefixColumn()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId], [p].[Descricao] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE [p].[ProdutoId] = @p0";

            // Act
            var query = SqlQuery.New()
                .Select(columnsTest, "p")
                .From("Produto p")
                .Where("p.ProdutoId", Condition.Equal, 1)
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
            Assert.Single(query.Parameters);
        }

        [Fact]
        public void WhereWithMultipeCondition()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId], [p].[Descricao] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE ([p].[ProdutoId] = @p0)";

            // Act
            var query = SqlQuery.New()
                .Select(columnsTest, "p")
                .From("Produto p")
                .Where(new[] {new Where("p.ProdutoId", Condition.Equal, "1")})
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
        }

        [Fact]
        public void Where_With_Multipe_Condition_OR()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId], [p].[Descricao] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE ([p].[ProdutoId] = @p0 OR [p].[Codigo] = @p1)";

            // Act
            var query = SqlQuery.New()
                .Select(columnsTest, "p")
                .From("Produto p")
                .Where(new[]
                {
                    new Where("p.ProdutoId", Condition.Equal, "1"),
                    new Where("p.Codigo", Condition.Equal, 1, LogicOperator.OR)
                })
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
            Assert.Equal(2, query.Parameters.Length);
        }

        [Fact]
        public void Where_With_WhereAnd_Simple()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId], [p].[Descricao] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE [p].[ProdutoId] = @p0 " +
                                         "AND [p].[Codigo] LIKE @p1";

            // Act
            var query = SqlQuery.New()
                .Select(columnsTest, "p")
                .From("Produto p")
                .Where("p.ProdutoId", Condition.Equal, "1")
                .WhereAnd("p.Codigo", Condition.Like, "10")
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
            Assert.Equal(2, query.Parameters.Length);
        }

        [Fact]
        public void Where_With_WhereAnd_Multiple_Column()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId], [p].[Descricao] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE [p].[ProdutoId] = @p0 " +
                                         "AND ([p].[Codigo] LIKE @p1 OR [p].[Grupo] = @p2)";

            // Act
            var query = SqlQuery.New()
                .From("Produto p")
                .Select(columnsTest, "p")
                .Where("p.ProdutoId", Condition.Equal, 1)
                .WhereAnd(new[]
                {
                    new Where("p.Codigo", Condition.Like, "10%"),
                    new Where("p.Grupo", Condition.Equal, "Grupo", LogicOperator.OR)
                })
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
            Assert.Equal(3, query.Parameters.Length);
        }

        [Fact]
        public void Where_With_WhereOR_Simple()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId], [p].[Descricao] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE [p].[ProdutoId] = @p0 " +
                                         "OR [p].[ProdutoId] = @p1";

            // Act
            var query = SqlQuery.New()
                .Select(columnsTest, "p")
                .From("Produto p")
                .Where("p.ProdutoId", Condition.Equal, "1")
                .WhereOr("p.ProdutoId", Condition.Equal, "2")
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
            Assert.Equal(2, query.Parameters.Length);
        }

        [Fact]
        public void Where_With_WhereOR_Multiple_Column()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId], [p].[Descricao] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE [p].[ProdutoId] = @p0 " +
                                         "OR ([p].[ProdutoId] = @p1 AND [p].[Descricao] = @p2)";

            // Act
            var query = SqlQuery.New()
                .Select(columnsTest, "p")
                .From("Produto p")
                .Where("p.ProdutoId", Condition.Equal, "1")
                .WhereOr(new[]
                {
                    new Where("p.ProdutoId", Condition.Equal, "1"),
                    new Where("p.Descricao", Condition.Equal, "2", LogicOperator.AND)
                })
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
            Assert.Equal(3, query.Parameters.Length);
        }

        [Fact]
        public void Where_With_Condition_true()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId] FROM [Produto] [p] WHERE [p].[ProdutoId] = @p0";

            // Act
            var query = SqlQuery.New()
                .Select("p.ProdutoId")
                .From("Produto p")
                .Where(true, "p.ProdutoId", Condition.Equal, 1)
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
        }

        [Fact]
        public void Where_With_Condition_false()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId] FROM [Produto] [p]";

            // Act
            var query = SqlQuery.New()
                .Select("p.ProdutoId")
                .From("Produto p")
                .Where(false, "p.ProdutoId", Condition.Equal, 1)
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
        }

        [Fact]
        public void Where_With_WhereAnd_Condition_true()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE [p].[ProdutoId] = @p0 AND [p].[Descricao] LIKE @p1";

            // Act
            var query = SqlQuery.New()
                .Select("p.ProdutoId")
                .From("Produto p")
                .Where("p.ProdutoId", Condition.Equal, 1)
                .WhereAnd(true, "p.Descricao", Condition.Like, "Descricao")
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
        }

        [Fact]
        public void Where_With_WhereAnd_Condition_false()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE [p].[ProdutoId] = @p0";

            // Act
            var query = SqlQuery.New()
                .Select("p.ProdutoId")
                .From("Produto p")
                .Where("p.ProdutoId", Condition.Equal, 1)
                .WhereAnd(false, "p.Descricao", Condition.Like, "Descricao")
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
        }

        [Fact]
        public void Where_With_WhereOr_Condition_true()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE [p].[ProdutoId] = @p0 OR [p].[Descricao] LIKE @p1";

            // Act
            var query = SqlQuery.New()
                .Select("p.ProdutoId")
                .From("Produto p")
                .Where("p.ProdutoId", Condition.Equal, 1)
                .WhereOr(true, "p.Descricao", Condition.Like, "Descricao")
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
        }

        [Fact]
        public void Where_With_WhereOr_Condition_false()
        {
            // Arrange
            const string queryExpected = "SELECT [p].[ProdutoId] " +
                                         "FROM [Produto] [p] " +
                                         "WHERE [p].[ProdutoId] = @p0";

            // Act
            var query = SqlQuery.New()
                .Select("p.ProdutoId")
                .From("Produto p")
                .Where("p.ProdutoId", Condition.Equal, 1)
                .WhereOr(false, "p.Descricao", Condition.Like, "Descricao")
                .Build();

            // Assert
            Assert.Equal(queryExpected, query.Command);
        }
    }
}