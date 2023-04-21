using ProceduralNoiseProject;
using UnityEngine;

public class VoronoiNoiseGenerator : MonoBehaviour{
    public int width = 256;
    public int height = 256;
    [SerializeField] private int seed = -1;
    [SerializeField] private float frequency;
    [SerializeField] private float amplitude = 1;
    [SerializeField] private int octaves = 4;
    public Texture2D noiseTexture;
    
    
    public Texture2D GenerateVoronoiNoiseTexture()
    {
        noiseTexture = new Texture2D(width, height);

        //Create the noise object and use a fractal to apply it.
        //The same noise object will be used for each fractal octave but you can 
        //manually set each individual ocatve like so...
        // fractal.Noises[3] = noise;
        INoise noise = new WorleyNoise(seed, frequency, amplitude);
        FractalNoise fractal = new FractalNoise(noise, octaves, frequency);

        float[,] arr = new float[width, height];

        //Sample the 2D noise and add it into a array.
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float fx = x / (width - 1.0f);
                float fy = y / (height - 1.0f);

                arr[x,y] = fractal.Sample2D(fx, fy);
            }
        }

        //Some of the noises range from -1-1 so normalize the data to 0-1 to make it easier to see.
        NormalizeArray(arr);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float n = arr[x, y];
                noiseTexture.SetPixel(x, y, new Color(n, n, n, 1));
            }
        }

        noiseTexture.Apply();
        return noiseTexture;
    }

    private void NormalizeArray(float[,] arr)
    {

        float min = float.PositiveInfinity;
        float max = float.NegativeInfinity;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                float v = arr[x, y];
                if (v < min) min = v;
                if (v > max) max = v;

            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float v = arr[x, y];
                arr[x, y] = (v - min) / (max - min);
            }
        }

    }
}