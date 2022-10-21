﻿using System.Text;
using TabAmp.Models;

namespace TabAmp.IO
{
    public class TabFileReader
    {
        private readonly TabFileTypesReader _typesReader;

        public TabFileReader(TabFileTypesReader typesReader) =>
            _typesReader = typesReader;

        public async Task<Song> ReadAsync()
        {
            var song = new Song();
            await ReadVersionAsync(song);
            return song;
        }

        private async Task ReadVersionAsync(Song song)
        {
            var versionBytes = new byte[4];
            for (var i = 0; i < 4; i++)
                versionBytes[i] = await _typesReader.ReadByteAsync();
            var versionString = Encoding.UTF8.GetString(versionBytes);
            if (versionString != "ABCD")
                throw new InvalidOperationException($"Invalid tab file version: {versionString}");
            song.Version = "GP510";
        }
    }
}
