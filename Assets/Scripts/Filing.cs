using UnityEngine;
using System.IO;
using System;

// Missing (Including but not limited to)
// 1. Ensure the size and format of the file is exactly equal to the size of all properties.
// 2. Dynamic, expandable property system that doesn't require implementation repetition
// 3. Atomic saving: Saving sequentially and having a fallback in case of exceptions
// 4. Data encryption for security
// 5. The real game state properties instead of example properties

// Field IO Order:
// 1) Position
// 2) Health

public class Filing : ScriptableObject
{
    private const string FileName = "state.bin";

    public class State // Example properties for now
    {
        public Vector2 position;
        public float health;
    }

    public static State Load()
    {
        if (!File.Exists(FileName))
            return Default();

        using var stream = File.Open(FileName, FileMode.Open);
        if (stream.Length == 0)
            return Default();

        using var reader = new BinaryReader(stream);

        State state = new State();

        state.position = BytesToVector2(reader.ReadBytes(sizeof(float) * 2));
        state.health = reader.ReadSingle();

        return state;
    }

    public static void Save(State state)
    {
        using var stream = File.Open(FileName, FileMode.Create);
        using var writer = new BinaryWriter(stream);

        writer.Write(Vector2ToBytes(state.position));
        writer.Write(state.health);
    }

    public static State Default()
    {
        State state = new State();

        state.position = Vector2.zero;
        state.health = 100f;

        return state;
    }

    private static byte[] Vector2ToBytes(Vector2 vector)
    {
        const int ComponentSize = sizeof(float);
        byte[] bytes = new byte[ComponentSize * 2];

        Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, bytes, ComponentSize * 0, ComponentSize);
        Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, bytes, ComponentSize * 1, ComponentSize);

        return bytes;
    }

    private static Vector2 BytesToVector2(byte[] bytes)
    {
        const int ComponentSize = sizeof(float);

        float x = BitConverter.ToSingle(bytes, ComponentSize * 0);
        float y = BitConverter.ToSingle(bytes, ComponentSize * 1);

        return new Vector2(x, y);
    }
}