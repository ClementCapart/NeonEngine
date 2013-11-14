using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using System.Reflection;

namespace NeonEngine
{
    public class Utils
    {
        public Utils()
        {
        }

        public float AngleBetween(Vector2 position1, Vector2 position2)
        {
            float angle = (float)Math.Atan2(position2.Y - position1.Y, position2.X - position1.X);
            angle = angle < 0 ? 2 * (float)Math.PI + angle : angle;
            return angle;
        }

        public Vector2 InputToWorldPosition(Vector2 input)
        {
            return (input - Neon.world.camera.Position - new Vector2(Neon.ScreenWidth / 2, Neon.ScreenHeight / 2));
        }

        public T[,] ArrayToArray2D<T>(T[] array, int dim1, int dim2)
        {
            T[,] convertedArray = new T[dim1, dim2];
            for (int x = 0; x < dim1; x++)
                for (int y = 0; y < dim2; y++)
                    convertedArray[y, x] = array[x + y * dim1];
            return convertedArray;
        }

        public T[] Array2DToArray<T>(T[,] array2D)
        {
            T[] convertedArray = new T[array2D.GetLength(0) * array2D.GetLength(1)];
            int index = 0;
            for (int x = 0; x < array2D.GetLength(0); x++)
                for (int y = 0; y < array2D.GetLength(1); y++)
                {
                    convertedArray[index] = array2D[x,y];
                    index++;
                }
            return convertedArray;
        }

        public Texture2D fillRectTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(Neon.graphicsDevice, width, height);
            Color[] fill = new Color[width * height];
            for (int i = 0; i < fill.Length; i++)
                fill[i] = color;
            texture.SetData(fill);
            return texture;
        }

        public Texture2D generateRadialGradient(int radius)
        {
            Texture2D t = new Texture2D(Neon.graphicsDevice, radius * 2, radius * 2);
            Vector2 center = new Vector2(radius);
            Color[,] data = new Color[t.Width, t.Height];
            for (int i = 0; i < radius * 2; i++)
            {
                for (int j = 0; j < radius * 2; j++)
                {
                    float alpha = 1 - (Vector2.Distance(center, new Vector2(i, j)) / radius);
                    data[i, j] = new Color(alpha, alpha, alpha, alpha);
                }
            }
            Color[] fill = Neon.utils.Array2DToArray<Color>(data);
            t.SetData(fill);
            return t;
        }

        public Entity GetEntityByBody(Body body)
        {
            foreach (Entity e in Neon.world.entities.Where(e => e.GetComponent<Rigidbody>() != null))
                if (e.GetComponent<Rigidbody>().body == body)
                    return e;

            return null;
        }

        public Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            if (assembly != null)
                return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
            else
                return new Type[0];
        }

        public Vector2 ParseVector2(string value)
        {
            Vector2 result = new Vector2();
            string CleanString = value.Replace("{X:", "");
            CleanString = CleanString.Replace("Y:", "");
            CleanString = CleanString.Replace("}", "");
            String[] Results = CleanString.Split(' ');
            result.X = float.Parse(Results[0]);
            result.Y = float.Parse(Results[1]);
            return result;
        }

        public Color ParseColor(string value)
        {
            string[] firstPass = value.Remove(0, 1).Remove(value.Length - 2, 1).Split(' ');
            return new Color(int.Parse(firstPass[0].Remove(0, 2)), int.Parse(firstPass[1].Remove(0, 2)), int.Parse(firstPass[2].Remove(0, 2)), int.Parse(firstPass[3].Remove(0, 2)));
        }
    }
}
