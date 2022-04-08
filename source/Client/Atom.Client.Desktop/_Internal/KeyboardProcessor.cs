using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Atom.Client.Desktop
{
    internal sealed class KeyboardProcessor
    {
        private readonly Dictionary<KeyCombintation, Action> _handlers;

        public KeyboardProcessor()
        {
            _handlers = new Dictionary<KeyCombintation, Action>();
        }

        public void Initialize()
        {
            Application.Current.MainWindow.KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            KeyCombintation keyCombintation = new KeyCombintation(e.Key, Keyboard.Modifiers);
            Action handler;
            if (_handlers.TryGetValue(keyCombintation, out handler))
            {
                handler();
            }
        }

        public void RegisterCommand(ModifierKeys modifier, Key key, Action handler)
        {
            KeyCombintation keyCombintation = new KeyCombintation(key, modifier);
            _handlers[keyCombintation] = handler;
        }

        private class KeyCombintation : IEquatable<KeyCombintation>
        {
            public KeyCombintation(Key key, ModifierKeys modifier)
            {
                Key = key;
                Modifier = modifier;
            }

            public Key Key { get; private set; }

            public ModifierKeys Modifier { get; private set; }

            public bool Equals(KeyCombintation other)
            {
                if (other == null)
                {
                    return false;
                }
                if (ReferenceEquals(this, other))
                {
                    return true;
                }
                return Key == other.Key && Modifier == other.Modifier;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as KeyCombintation);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + Key.GetHashCode();
                    hash = hash * 23 + Modifier.GetHashCode();
                    return hash;
                }
            }

            public override string ToString()
            {
                return $"{Modifier} + {Key}";
            }
        }
    }
}
