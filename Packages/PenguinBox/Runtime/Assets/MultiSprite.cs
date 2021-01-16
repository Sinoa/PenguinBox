// Zlib license
//
// Copyright (c) 2021 Sinoa
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
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Sinoalmond.PenguinBox.Assets
{
    /// <summary>
    /// Unityの MultiSprite としてセットアップされたアセットを扱うクラスです
    /// </summary>
    public class MultiSprite : ScriptableObject
    {
        [NonSerialized]
        private Dictionary<string, Sprite> spriteTable;



        /// <summary>
        /// このマルチスプライトに含まれるスプライト名のコレクションを取得します
        /// </summary>
        public Dictionary<string, Sprite>.KeyCollection SpriteNames => spriteTable.Keys;


        /// <summary>
        /// このマルチスプライトに含まれるスプライトのコレクションを取得します
        /// </summary>
        public Dictionary<string, Sprite>.ValueCollection Sprites => spriteTable.Values;


        /// <summary>
        /// スプライト名からスプライトへアクセスします
        /// </summary>
        /// <param name="name">アクセスするスプライト名</param>
        /// <returns>指定された名前のスプライトを返しますが、存在しない場合は null を返します</returns>
        public Sprite this[string name]
        {
            get
            {
                spriteTable.TryGetValue(name, out var sprite);
                return sprite;
            }
        }



        private void Awake()
        {
            spriteTable = new Dictionary<string, Sprite>();
        }


        internal void SetSprites(UnityObject[] objects)
        {
            SetSprites(Array.ConvertAll(objects, x => (Sprite)x));
        }


        internal void SetSprites(Sprite[] sprites)
        {
            spriteTable.Clear();
            foreach (var sprite in sprites ?? throw new ArgumentNullException(nameof(sprites)))
            {
                spriteTable[sprite.name] = sprite;
            }
        }
    }
}