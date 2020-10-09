﻿using System;
using Impostor.Shared.Innersloth;

namespace Impostor.Server
{
    public readonly struct GameCode : IEquatable<GameCode>
    {
        public GameCode(int value)
        {
            Value = value;
            Code = GameCodeParser.IntToGameName(value);
        }

        public GameCode(string code)
        {
            Value = GameCodeParser.GameNameToInt(code);
            Code = code;
        }
        
        public string Code { get; }
        
        public int Value { get; }

        public static implicit operator string(GameCode code) => code.Code;

        public static implicit operator int(GameCode code) => code.Value;

        public static implicit operator GameCode(string code) => From(code);

        public static implicit operator GameCode(int value) => From(value);

        public bool Equals(GameCode other)
        {
            return Code == other.Code && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is GameCode other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Value);
        }

        public static bool operator ==(GameCode left, GameCode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GameCode left, GameCode right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return Code;
        }

        public static GameCode From(int value) => new GameCode(value);

        public static GameCode From(string value) => new GameCode(value);

        public static GameCode Create()
        {
            return new GameCode(GameCodeParser.GenerateCode(6));
        }
    }
}