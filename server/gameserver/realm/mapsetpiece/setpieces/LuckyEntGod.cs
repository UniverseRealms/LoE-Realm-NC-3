﻿namespace LoESoft.GameServer.realm.mapsetpiece
{
    internal class LuckyEntGod : MapSetPiece
    {
        public override int Size => 5;

        public override void RenderSetPiece(World world, IntPoint pos)
        {
            Entity cube = Entity.Resolve("Lucky Ent God");
            cube.Move(pos.X + 2.5f, pos.Y + 2.5f);
            world.EnterWorld(cube);
        }
    }
}