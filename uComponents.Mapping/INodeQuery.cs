﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using umbraco.NodeFactory;

namespace uComponents.Mapping
{
    /// <summary>
    /// Represents a query for mapped Umbraco nodes.  Enumerating the
    /// query gets mapped instances of every node which can be mapped to 
    /// <typeparamref name="TDestination"/>. 
    /// </summary>
    /// <typeparam name="TDestination">
    /// The type which queried nodes will be mapped to.
    /// </typeparam>
    public interface INodeQuery<TDestination> : IEnumerable<TDestination>
        where TDestination : class, new()
    {
        /// <summary>
        /// Gets a mapped instance of a specific instance of a <c>Node</c>.
        /// </summary>
        /// <param name="node">The node to map from.</param>
        /// <returns>
        /// the mapped node, or null if <paramref name="node"/> is
        /// the 'empty' node or null.
        /// </returns>
        TDestination Map(Node node);

        /// <summary>
        /// Gets a mapped instance of a node by it's ID.
        /// </summary>
        /// <param name="nodeId">The ID of the node to map.</param>
        /// <returns>The mapped node, or null if it does not exist.</returns>
        TDestination Single(int nodeId);

        /// <summary>
        /// Gets mapped instances of nodes by their ID.
        /// </summary>
        /// <param name="nodeIds">The IDs of the nodes to map.</param>
        /// <returns>
        /// A one-to-one collection of mapped nodes to node IDs
        /// (there will be holes in the collection if some nodes
        /// do not exist).
        /// </returns>
        IEnumerable<TDestination> Many(IEnumerable<int> nodeIds);

        /// <summary>
        /// Gets mapped instances of <c>Node</c>s.
        /// </summary>
        /// <param name="nodes">The nodes to map.</param>
        /// <returns>
        /// A one-to-one collection of mapped nodes to nodes
        /// (there will be holes in the collection if some nodes
        /// do not exist).
        /// </returns>
        IEnumerable<TDestination> Many(IEnumerable<Node> nodes);

        /// <summary>
        /// Gets a mapped instance of the current node.
        /// </summary>
        /// <returns>
        /// The mapped node, or null if there is no
        /// current node.
        /// </returns>
        TDestination Current();

        /// <summary>
        /// Gets mapped instances only of nodes which were mapped to
        /// <typeparamref name="TDestination"/> using 
        /// <see cref="INodeMappingEngine.CreateMap()"/>.
        /// </summary>
        /// <remarks>
        /// Will not include node type aliases which map to a model
        /// which can be cast to <typeparamref name="TDestination"/>.
        /// </remarks>
        /// <returns>
        /// A collection of mapped nodes of type
        /// <typeparamref name="TDestination"/>.
        /// </returns>
        IEnumerable<TDestination> Explicit();

        /// <summary>
        /// Exposes the engine being used under the hood.  Useful if you want to
        /// write extension methods for <c>INodeQuery`</c>.
        /// </summary>
        INodeMappingEngine Engine { get; }

        /// <summary>
        /// Includes a relationship path in the node query.
        /// </summary>
        /// <param name="path">
        /// The path of the relationship to include, 
        /// e.g. "Employers.Employees.GoldStars"
        /// </param>
        /// <example>
        /// <code>
        /// // If querying on a set of companies,
        /// var myQuery = uMapper.Set&lt;Company&gt;()
        ///     .Include("Employers.Employees.GoldStars")
        ///     .Include("Employers.Employees.FavouriteMeals");
        /// </code>
        /// </example>
        /// <returns>
        /// The query with the <paramref name="path"/> 
        /// included.
        /// </returns>
        INodeQuery<TDestination> Include(string path);

        /// <summary>
        /// Includes a relationship path in the node query.
        /// </summary>
        /// <remarks>
        /// Remarks from <c>System.Data.Entity.IQueryableExtensions.Include`2</c>:
        /// 
        /// The path expression must be composed of simple property access expressions together with calls to Select for
        /// composing additional includes after including a collection proprty.  Examples of possible include paths are:
        /// To include a single reference: query.Include(e => e.Level1Reference)
        /// To include a single collection: query.Include(e => e.Level1Collection)
        /// To include a reference and then a reference one level down: query.Include(e => e.Level1Reference.Level2Reference)
        /// To include a reference and then a collection one level down: query.Include(e => e.Level1Reference.Level2Collection)
        /// To include a collection and then a reference one level down: query.Include(e => e.Level1Collection.Select(l1 => l1.Level2Reference))
        /// To include a collection and then a collection one level down: query.Include(e => e.Level1Collection.Select(l1 => l1.Level2Collection))
        /// To include a collection and then a reference one level down: query.Include(e => e.Level1Collection.Select(l1 => l1.Level2Reference))
        /// To include a collection and then a collection one level down: query.Include(e => e.Level1Collection.Select(l1 => l1.Level2Collection))
        /// To include a collection, a reference, and a reference two levels down: query.Include(e => e.Level1Collection.Select(l1 => l1.Level2Reference.Level3Reference))
        /// To include a collection, a collection, and a reference two levels down: query.Include(e => e.Level1Collection.Select(l1 => l1.Level2Collection.Select(l2 => l2.Level3Reference)))
        /// </remarks>
        /// <typeparam name="TProperty">
        /// The type of the top-level property in the path.
        /// </typeparam>
        /// <param name="path">
        /// The path of the relationship to include, defined using
        /// a lambda expression.
        /// </param>
        /// <returns>
        /// The query with the <paramref name="path"/> 
        /// included.
        /// </returns>
        INodeQuery<TDestination> Include<TProperty>(Expression<Func<TDestination, TProperty>> path);

        /// <summary>
        /// Includes many relationship paths in the node query
        /// </summary>
        /// <seealso cref="Include"/>
        INodeQuery<TDestination> IncludeMany(string[] paths);
        
        /// <summary>
        /// Includes many relationship paths in the node query
        /// </summary>
        /// <seealso cref="Include{TProperty}"/>
        INodeQuery<TDestination> IncludeMany(Expression<Func<TDestination, object>>[] paths);
    }
}