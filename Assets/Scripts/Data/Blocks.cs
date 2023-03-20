namespace VoxelEngine.Data
{
    public static class Blocks
    {
        public static Block Air = new AirBlock() { id = 0 };
        public static Block Bedrock = new BedrockBlock() { id = 1 };
        public static Block Stone = new StoneBlock() { id = 2 };
        public static Block Dirt = new DirtBlock() { id = 3 };
        public static Block Grass = new GrassBlock() { id = 4 };
        public static Block Sand = new SandBlock() { id = 5 };
        public static Block Log = new LogBlock() { id = 6 };
        public static Block Leaves = new LeavesBlock() { id = 7 };
        public static Block Cactus = new CactusBlock() { id = 8 };
        public static Block DeadBush = new DeadBushBlock() { id = 9 };
        public static Block Flower = new FlowerBlock() { id = 10 };

        public static Block CoalOre = new CoalOreBlock() { id = 11 };
        public static Block IronOre = new IronOreBlock() { id = 12 };

        public static Block Glass = new GlassBlock() { id = 13 };
        public static Block Plank = new PlankBlock() { id = 14 };

        public static Block[] BLOCKS = {
            Air,
            Bedrock,
            Stone,
            Dirt,
            Grass,
            Sand,
            Log,
            Leaves,
            Cactus,
            DeadBush,
            Flower,
            CoalOre,
            IronOre,
            Glass,
            Plank
        };
    }
}