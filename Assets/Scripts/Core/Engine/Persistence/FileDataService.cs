using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Persistence
{
    public class FileDataService : IDataService
    {
        private ISerializer _serializer;
        private string _dataPath;
        private string _fileExtension;

        public FileDataService(ISerializer serializer)
        {
            _dataPath = Application.persistentDataPath;
            _fileExtension = ".json";
            _serializer = serializer;
        }

        #region Public Methods
        public void Save(GameData data, bool overwrite = false)
        {
            string fileLocation = GetPathToFile(data.SlotName);

            if (!overwrite && File.Exists(fileLocation))
            {
                throw new IOException($"The file '{data.SlotName}{_fileExtension}' already exists and cannot be overwritten.");
            }

            File.WriteAllText(fileLocation, _serializer.Serialize(data));
        }

        public GameData Load(string name)
        {
            string fileLocation = GetPathToFile(name);

            if (!File.Exists(fileLocation))
            {
                throw new ArgumentException($"No persisted GameData with name '{name}'");
            }

            return _serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        }

        public void Delete(string name)
        {
            string fileLocation = GetPathToFile(name);

            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
        }

        public void DeleteAll()
        {
            foreach (string filePath in Directory.GetFiles(_dataPath))
            {
                File.Delete(filePath);
            }
        }

        public List<string> ListSaves()
        {
            var saves = new List<string>();

            foreach (string path in Directory.EnumerateFiles(_dataPath))
            {
                if (Path.GetExtension(path) == _fileExtension)
                    saves.Add(Path.GetFileNameWithoutExtension(path));
            }

            return saves;
        }
        #endregion

        #region Private Methods
        private string GetPathToFile(string fileName)
        {
            return Path.Combine(_dataPath, string.Concat(fileName, _fileExtension));
        }
        #endregion
    }
}