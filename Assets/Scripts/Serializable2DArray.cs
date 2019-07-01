namespace WGJ.PuppetShadow
{

    using System;

    [Serializable]
    public class Serializable2DArray<T>
    {
        public int Width;
        public int Height;
        public T[] Array;

        public T this[int i, int j]
        {
            get
            {
                return Array[(j * Width) + i];
            }
            set
            {
                Array[(j * Width) + i] = value;
            }
        }

        public Serializable2DArray(int width, int height)
        {
            Width = width;
            Height = height;
            Array = new T[width * height];
        }
    }

}


