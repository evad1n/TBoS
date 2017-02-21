﻿using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone
{
    class Collectible : Entity
    {
        public Collectible(World world, Shape shape, Vector2 position, Texture2D texture)
            :base (world, shape, position, texture, null)
        {

        }
    }
}