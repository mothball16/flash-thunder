using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Managers
{
    /// <summary>
    /// Handles the loading and retrieving of assets.
    /// </summary>
    public class AssetManager
    {
        private char DefaultRep = '&';
        private Dictionary<char, Texture2D> _texCache;

        public AssetManager()
        {
            _texCache = [];
        }


        public AssetManager RegTex(Texture2D texture)
            => RegTex(DefaultRep, texture);
        public AssetManager RegTex(char name, Texture2D texture)
        {
            if (_texCache.ContainsKey(name))
            {
                Console.WriteLine(
                    $"[WARNING] Texture {name} has already been bound." +
                    $"Try not to do this because it is bad. Bad");
            }
            else
            {
                _texCache.Add(name, texture);
            }
            return this;
        }

        /// <summary>
        /// Attempts to retrieve the texture by name.
        /// </summary>
        /// <param name="name">The name the texture should be mapped to.</param>
        /// <returns>The Texture2D object mapped to the name.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Throws if neither tex or default could be found.
        /// </exception>
        public Texture2D GetTex(char name)
        {
            if(!_texCache.TryGetValue(name, out Texture2D tex))
            {
                Console.WriteLine(
                    $"[WARNING] Texture {name} wasn't found in the texture cache." +
                    $"Attempting to retrieve default instead.");

                //if we don't even have a default, throw an exception
                if (!_texCache.TryGetValue(DefaultRep, out Texture2D defaultTile))
                {
                    throw new KeyNotFoundException(
                        $"No default texture was found to replacing missing texture {name}.");   
                }

                //set the tex to default (we know that it does exist now)
                tex = defaultTile;
            }
            return tex;
        }

        /// <summary>
        /// Clears all caches.
        /// </summary>
        public void Clear(bool clearDefault = false)
        {
            Texture2D temp = null;
            if (!clearDefault && _texCache.TryGetValue(DefaultRep, out Texture2D val))
            {
                temp = val;
            }
            _texCache.Clear();

            if (temp != null)
            {
                _texCache[DefaultRep] = temp;
            }
        }
    }
}
