using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    /// <summary>
    /// Reads and interprets image map files for passing between the chunk generator and the chunk itself.
    /// 
    /// By Dom Liotti and Will Dickinson
    /// </summary>
    public static class MapReader {

        /// <summary>
        /// Reads in an image from a given path relative to the directory .\Content\maps\ (of the executable) and converts it to a 2D array of tile IDs.
        /// </summary>
        /// <param name="imagePath">The relative path of the image, starting from .\Content\maps\ in the application's current directory.</param>
        /// <returns>A 2D array of tile IDs.</returns>
        public static int[,] ReadImage(string imagePath) {
            string path = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\Content\maps\" + imagePath));
            Console.WriteLine(newPath);
            Bitmap img = new Bitmap(newPath); //Get and convert the file to a readable bitmap.

            int[,] atlas = new int[img.Height, img.Width]; //Initialize dimensions of returned array

            for (int x = 0; x < atlas.GetLength(1); x++) { //Iterate through each atlas index
                for (int y = 0; y < atlas.GetLength(0); y++) {
                    System.Drawing.Color px = img.GetPixel(x, y); //Get the pixel color at this index and convert it to a string

                    string pxStr =
                        px.R.ToString("D3") + " " +
                        px.G.ToString("D3") + " " +
                        px.B.ToString("D3") + " " +
                        px.A.ToString("D3");

                    atlas[y, x] = TileID(pxStr, x, img.Width); //Convert the pixel color to an integer ID and populate the array
                }
            }

            return atlas; //Return the complete array
        }

        /// <summary>
        /// Returns the integer tile ID from a given color, the relative x position within the chunk, and the width of the chunk.
        /// </summary>
        /// <param name="color">The color of the tile, in RGBA "### ### ### ###" format.</param>
        /// <param name="x">The x position of the tile within the chunk.</param>
        /// <param name="width">The width of the chunk.</param>
        /// <returns>The integer tile ID.</returns>
        static int TileID(string color, int x = -1, int width = -1) {
            int tileID = 0;

            switch (color) {
                case "092 186 072 255":
                    tileID = 1; //Ground tile
                    break;

                case "105 119 135 255":
                    tileID = 2; //Background tile
                    break;

                case "153 175 173 255":
                    tileID = 3; //stone tile
                    break;

                case "255 205 000 255":
                    tileID = 6; //spawns a coin pickup on open air
                    break;

                case "255 174 012 255":
                    tileID = 7; //spawns a coin pickup with a background tile behind it
                    break;

                case "255 000 000 255":
                    //use the chunk width to determine whether this red pixel is on the left side or the right side of the atlas
                    if (x == 0)
                        tileID = 4; //Start tile (grass tile with bool isStartTile = true)
                    else if (x == width - 1)
                        tileID = 5; //End tile (grass tile with bool isEndTile = true)
                    else
                        tileID = 1; //Otherwise, the red tile is extraneous and is treated as a regular grass tile
                    break;

                case "255 000 255 255":
                    tileID = 8; //spawns a ground enemy without a background tile
                    break;

                case "150 000 150 255":
                    tileID = 9; //spawns a ground enemy with a background tile
                    break;

                case "255 076 000 255":
                    tileID = 10; //spawns a vertical spike tile
                    break;

                case "167 043 000 255":
                    tileID = 11; //spawns a vertical spike tile w/ bkd tile
                    break;

                case "255 148 000 255":
                    tileID = 12; //spawns a horizontal spike tile
                    break;

                case "167 090 000 255":
                    tileID = 13; //spawns a horizontal spike tile w/ bkd tile
                    break;
            }
            return tileID;
        }
    }
}
