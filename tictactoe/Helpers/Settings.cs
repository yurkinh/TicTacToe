﻿using System;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Preferences;
using Tictactoe.Constants;

namespace Tictactoe.Helpers
{   
    public static class Settings
    {
        static readonly object locker = new object();
        const string sharedName = StringConstants.TTT_SETTINGS;

        public static bool ContainsKey(string key)
        {
            lock (locker)
            {
                using (var sharedPreferences = GetSharedPreferences())
                {
                    return sharedPreferences.Contains(key);
                }
            }
        }

        public static void Remove(string key)
        {
            lock (locker)
            {
                using (var sharedPreferences = GetSharedPreferences())
                using (var editor = sharedPreferences.Edit())
                {
                    editor.Remove(key).Apply();
                }
            }
        }

        public static void Clear()
        {
            lock (locker)
            {
                using (var sharedPreferences = GetSharedPreferences())
                using (var editor = sharedPreferences.Edit())
                {
                    editor.Clear().Apply();
                }
            }
        }

        public static void Set<T>(string key, T value)
        {
            lock (locker)
            {
                using (var sharedPreferences = GetSharedPreferences())
                using (var editor = sharedPreferences.Edit())
                {
                    if (value == null)
                    {
                        editor.Remove(key);
                    }
                    else
                    {
                        switch (value)
                        {
                            case string s:
                                editor.PutString(key, s);
                                break;
                            case int i:
                                editor.PutInt(key, i);
                                break;
                            case bool b:
                                editor.PutBoolean(key, b);
                                break;
                            case long l:
                                editor.PutLong(key, l);
                                break;
                            case double d:
                                var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                                editor.PutString(key, valueString);
                                break;
                            case float f:
                                editor.PutFloat(key, f);
                                break;
                        }
                    }
                    editor.Apply();
                }
            }
        }

        public static T Get<T>(string key, T defaultValue)
        {
            lock (locker)
            {
                object value = null;
                using (var sharedPreferences = GetSharedPreferences())
                {
                    if (defaultValue == null)
                    {
                        value = sharedPreferences.GetString(key, null);
                    }
                    else
                    {
                        switch (defaultValue)
                        {
                            case int i:
                                value = sharedPreferences.GetInt(key, i);
                                break;
                            case bool b:
                                value = sharedPreferences.GetBoolean(key, b);
                                break;
                            case long l:
                                value = sharedPreferences.GetLong(key, l);
                                break;
                            case double d:
                                var savedDouble = sharedPreferences.GetString(key, null);
                                if (string.IsNullOrWhiteSpace(savedDouble))
                                {
                                    value = defaultValue;
                                }
                                else
                                {
                                    if (!double.TryParse(savedDouble, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out var outDouble))
                                    {
                                        var maxString = Convert.ToString(double.MaxValue, CultureInfo.InvariantCulture);
                                        outDouble = savedDouble.Equals(maxString) ? double.MaxValue : double.MinValue;
                                    }

                                    value = outDouble;
                                }
                                break;
                            case float f:
                                value = sharedPreferences.GetFloat(key, f);
                                break;
                            case string s:
                                // the case when the string is not null
                                value = sharedPreferences.GetString(key, s);
                                break;
                        }
                    }
                }

                return (T)value;
            }
        }

        static ISharedPreferences GetSharedPreferences()
        {
            var context = Application.Context;

            return string.IsNullOrWhiteSpace(sharedName) ?
                PreferenceManager.GetDefaultSharedPreferences(context) :
                    context.GetSharedPreferences(sharedName, FileCreationMode.Private);
        }
    }
}
