using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using MusicBeePlugin.AndroidRemote.Data;
using MusicBeePlugin.AndroidRemote.Entities;
using MusicBeePlugin.AndroidRemote.Networking;
using MusicBeePlugin.AndroidRemote.Utilities;

namespace MusicBeePlugin
{
    public class SyncModule : Messenger
    {
        
        private CacheHelper mHelper;
        private Plugin.MusicBeeApiInterface api;
        private List<LibraryData> mData;
        private List<String> coverHashList;

        public SyncModule(Plugin.MusicBeeApiInterface api, String storagePath)
        {
            this.api = api;
            mHelper = new CacheHelper(storagePath);
        }

        public void SyncCheckForChanges(string[] cachedFiles ,DateTime lastSync)
        {
            string[] newFiles = {};
            string[] deletedFiles = {};
            string[] updatedFiles ={};

            api.Library_GetSyncDelta(cachedFiles, lastSync, Plugin.LibraryCategory.Music,
                ref newFiles, ref updatedFiles, ref deletedFiles);

            var jsonData = new
            {
                type = "partial",
                update = updatedFiles,
                deleted = deletedFiles,
                newfiles = newFiles
            };

            SendSocketMessage(Constants.LibrarySync, Constants.Reply, jsonData);
        }

        public void SyncGetFilenames(string clientId)
        {
            string[] files = {};
            api.Library_QueryFilesEx(String.Empty, ref files);
            mData = mHelper.GetCachedFiles();
            var jsonData = new
            {
                type = "full",
                payload = files.Length
            };

            SendSocketMessage(Constants.LibrarySync, Constants.Reply, jsonData, clientId);
        }

        public void SyncGetCover(string hash, string clientId)
        {
            coverHashList = mHelper.GetCoverHashes();

        }

        public void BuildCache()
        {
            string[] files = {};
            api.Library_QueryFilesEx(String.Empty, ref files);
            mHelper.CreateCache(files);
        }

        public void BuildCoverCache()
        {
            var update = new List<LibraryData>();
            var total = mHelper.GetCachedFiles();

            foreach (var entry in total)
            {
                var cover = api.Library_GetArtworkUrl(entry.Filepath, 0);
                entry.CoverHash = Utilities.CacheArtworkImage(cover);
                update.Add(entry);
            }   
            mHelper.UpdateImageCache(update);
        }

        public void BuildArtistCoverCache()
        {
            List<Artist> artistList = new List<Artist>();
            if (api.Library_QueryLookupTable("artist", "count", ""))
            {
                foreach (string entry in api.Library_QueryGetLookupTableValue(null).Split(new[] {"\0\0"}, StringSplitOptions.None))
                {
                    string[] artistInfo = entry.Split(new[] { '\0' });
                    artistList.Add(new Artist(artistInfo[0], Int32.Parse(artistInfo[1])));
                }
            }

            api.Library_QueryLookupTable(null, null, null);
            foreach (var entry in artistList)
            {
                string[] urls = {};
                var artist = entry.artist;
                api.Library_GetArtistPictureUrls(artist, true, ref urls);
                if (urls.Length <= 0) continue;
                var hash = Utilities.CacheArtistImage(urls[0], artist);
                mHelper.CacheArtistUrl(artist, hash);
            }   
            
        }

        public void SyncGetCovers(int index, string client, int limit = 5)
        {
            var buffer = new List<ImageData>();
            string imageData;
            do
            {
                var hash = coverHashList[index];
                imageData = Utilities.GetCachedImage(hash);
                var image = new ImageData(hash, imageData);
                buffer.Add(image);
                index++;
            } while (index < coverHashList.Count && buffer.Count < limit);

            var pack = new
            {
                type = "cover",
                data = buffer
            };

            SendSocketMessage(Constants.LibrarySync, Constants.Reply, pack, client);
        }
               
        public void SyncGetMetaData(int offset, string client, int limit = 50)
        {
            var buffer = new List<MetaData>();
            LibraryData entry;
            do
            {
                entry = mData[offset];
                var file = entry.Filepath;
                var meta = new MetaData {hash = entry.Hash, file = file};

                if (Plugin.MusicBeeVersion.v2_2 == api.MusicBeeVersion)
                {
                    meta.artist = api.Library_GetFileTag(file, Plugin.MetaDataType.Artist);
                    meta.album_artist = api.Library_GetFileTag(file, Plugin.MetaDataType.AlbumArtist);
                    meta.album = api.Library_GetFileTag(file, Plugin.MetaDataType.Album);
                    meta.title = api.Library_GetFileTag(file, Plugin.MetaDataType.TrackTitle);
                    meta.genre = api.Library_GetFileTag(file, Plugin.MetaDataType.Genre);
                    meta.year = api.Library_GetFileTag(file, Plugin.MetaDataType.Year);
                    meta.track_no = api.Library_GetFileTag(file, Plugin.MetaDataType.TrackNo);
                }
                else
                {
                    Plugin.MetaDataType[] types =
                    {
                        Plugin.MetaDataType.Artist, Plugin.MetaDataType.AlbumArtist, Plugin.MetaDataType.Album, Plugin.MetaDataType.TrackTitle, Plugin.MetaDataType.Genre, Plugin.MetaDataType.Year, Plugin.MetaDataType.TrackNo
                    };
                    var i = 0;
                    string[] tags = {};
                    api.Library_GetFileTags(file, types, ref tags);
                    meta.artist = tags[i++];
                    meta.album_artist = tags[i++];
                    meta.album = tags[i++];
                    meta.title = tags[i++];
                    meta.genre = tags[i++];
                    meta.year = tags[i++];
                    meta.track_no = tags[i];
                }
                offset++;
                buffer.Add(meta);
            } while (entry != null && offset < mData.Count && buffer.Count < limit);

            if (offset == mData.Count + 1)
            {
                mData = null;
            }

            var pack = new
            {
                type = "meta",
                data = buffer
            };

            SendSocketMessage(Constants.LibrarySync, Constants.Reply, pack, client);
        }
    }
}