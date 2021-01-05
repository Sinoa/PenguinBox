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

using System.Collections.Generic;

namespace Sinoalmond.PenguinBox.Utilities
{
    /// <summary>
    /// 循環参照検知付きのオブジェクトの依存順序解決クラスです
    /// </summary>
    /// <typeparam name="TObject">扱うオブジェクトの型</typeparam>
    public class DependenceOrderDetector<TObject>
    {
        private readonly Dictionary<TObject, Node> nodeTable;



        /// <summary>
        /// 現在の解決するべきオブジェクトの数
        /// </summary>
        public int ObjectCount => nodeTable.Count;



        /// <summary>
        /// DependenceOrderDetector クラスのインスタンスを初期化します
        /// </summary>
        public DependenceOrderDetector()
        {
            nodeTable = new Dictionary<TObject, Node>();
        }


        /// <summary>
        /// このインスタンス状態をリセットします
        /// </summary>
        public void Reset()
        {
            nodeTable.Clear();
        }


        /// <summary>
        /// 単純な依存関係を追加します
        /// </summary>
        /// <param name="from">関係の参照元オブジェクト</param>
        /// <param name="to">関係の参照先オブジェクト</param>
        public void AddRelation(TObject from, TObject to)
        {
            GetOrCreateNode(from).AddChild(to);
            GetOrCreateNode(to).IncrementReference();
        }


        /// <summary>
        /// 1:n な依存関係を追加します
        /// </summary>
        /// <param name="from">関係の参照元オブジェクト</param>
        /// <param name="toEnumerable">関係の参照先となるオブジェクト列挙オブジェクト</param>
        public void AddRelations(TObject from, IEnumerable<TObject> toEnumerable)
        {
            var fromNode = GetOrCreateNode(from);
            foreach (var to in toEnumerable)
            {
                fromNode.AddChild(to);
                GetOrCreateNode(to).IncrementReference();
            }
        }


        /// <summary>
        /// 追加した依存関係を元に依存順序を解決します。
        /// また、この関数の実行後は成否問わず無条件で内部状態がリセットされます。
        /// </summary>
        /// <returns>正しく依存順序を解決した場合は、依存の浅い順に並んだ列挙可能オブジェクトを返しますが、循環参照など順序を解決出来ない場合は null を返します</returns>
        public IEnumerable<TObject> Detect()
        {
            var rootQueue = new Queue<Node>(nodeTable.Count);
            var sortedList = new List<TObject>(nodeTable.Count);


            foreach (var node in nodeTable.Values)
            {
                if (node.ReferenceCount == 0)
                {
                    rootQueue.Enqueue(node);
                }
            }


            while (rootQueue.Count > 0)
            {
                var rootNode = rootQueue.Dequeue();
                sortedList.Add(rootNode.Object);
                nodeTable.Remove(rootNode.Object);


                foreach (var child in rootNode.Childlen)
                {
                    var childNode = GetNode(child);
                    childNode.DecrementReference();
                    if (childNode.ReferenceCount == 0)
                    {
                        rootQueue.Enqueue(childNode);
                    }
                }
            }


            if (nodeTable.Count > 0)
            {
                Reset();
                return null;
            }


            return sortedList;
        }


        private Node GetNode(TObject obj)
        {
            nodeTable.TryGetValue(obj, out var node);
            return node;
        }


        private Node GetOrCreateNode(TObject obj)
        {
            if (!nodeTable.TryGetValue(obj, out var node))
            {
                node = new Node(obj);
                nodeTable[obj] = node;
            }


            return node;
        }



        private class Node
        {
            private readonly HashSet<TObject> childlen;



            public TObject Object { get; }
            public IEnumerable<TObject> Childlen => childlen;
            public int ReferenceCount { get; private set; }



            public Node(TObject obj)
            {
                Object = obj;
                ReferenceCount = 0;
                childlen = new HashSet<TObject>();
            }


            public void IncrementReference()
            {
                ++ReferenceCount;
            }


            public void DecrementReference()
            {
                --ReferenceCount;
            }


            public void AddChild(TObject child)
            {
                childlen.Add(child);
            }
        }
    }
}