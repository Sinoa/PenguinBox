// Zlib license
//
// Copyright (c) 2020 Sinoa
//
// This software is provided 'as-is', without any express or implied warranty.
// In no event will the authors be held liable for any damages arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it freely,
// subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software.
//    If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.

using System;
using System.Collections.Generic;

namespace Sinoalmond.PenguinBox.Utilities
{
    public class DependenciesDetector<TKey, TNode>
    {
        private readonly Dictionary<TKey, NodeReference> referenceTable;
        private readonly Func<TNode, TKey> keySelector;
        private readonly Func<TNode, IEnumerable<TNode>> childIterator;



        public DependenciesDetector(Func<TNode, TKey> keySelector, Func<TNode, IEnumerable<TNode>> childIterator)
        {
            this.keySelector = keySelector;
            this.childIterator = childIterator;
            referenceTable = new Dictionary<TKey, NodeReference>();
        }


        public void Reset()
        {
            referenceTable.Clear();
        }


        public void AddRelation(TNode from, TNode to)
        {
            var key = keySelector(to);
            if (!referenceTable.TryGetValue(key, out var value))
            {
                value = new NodeReference(to);
                referenceTable[key] = value;
            }


            value.ReferenceCount++;


            key = keySelector(from);
            if (!referenceTable.ContainsKey(key))
            {
                value = new NodeReference(from);
                referenceTable[key] = value;
            }
        }


        public void AddNode(TNode rootNode, bool recursive)
        {
            var temporaryStack = new Stack<(TNode node, IEnumerator<TNode> iterator)>();
            temporaryStack.Push((rootNode, childIterator(rootNode).GetEnumerator()));
            while (temporaryStack.Count > 0)
            {
                var parent = temporaryStack.Pop();
                var fromNode = parent.node;
                var iterator = parent.iterator;
                while (iterator.MoveNext())
                {
                    var toNode = iterator.Current;
                    AddRelation(fromNode, toNode);
                }
            }
        }



        private class NodeReference
        {
            public TNode Node { get; }
            public int ReferenceCount { get; set; }



            public NodeReference(TNode node)
            {
                Node = node;
                ReferenceCount = 0;
            }
        }
    }
}