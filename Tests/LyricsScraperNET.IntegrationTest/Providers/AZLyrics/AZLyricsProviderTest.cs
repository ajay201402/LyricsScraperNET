﻿using LyricsScraperNET.Models.Requests;
using LyricsScraperNET.Models.Responses;
using LyricsScraperNET.Providers.AZLyrics;
using LyricsScraperNET.Providers.Models;
using LyricsScraperNET.TestShared.Providers;
using LyricsScraperNET.UnitTest.TestModel;
using System.Threading;
using Xunit;

namespace LyricsScraperNET.IntegrationTest.Providers.AZLyrics
{
    public class AZLyricsProviderTest : ProviderTestBase
    {
        [Theory]
        [MemberData(nameof(GetTestData), parameters: "Providers\\AZLyrics\\lyric_test_data.json")]
        public void SearchLyric_IntegrationDynamicData_Success(LyricsTestData testData)
        {
            // Arrange
            var lyricsClient = new AZLyricsProvider();
            SearchRequest searchRequest = CreateSearchRequest(testData);
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var searchResult = lyricsClient.SearchLyric(searchRequest, cancellationToken);

            // Assert
            Assert.NotNull(searchResult);
            Assert.False(searchResult.IsEmpty());
            Assert.Equal(ResponseStatusCode.Success, searchResult.ResponseStatusCode);
            Assert.True(string.IsNullOrEmpty(searchResult.ResponseMessage));
            Assert.Equal(ExternalProviderType.AZLyrics, searchResult.ExternalProviderType);
            Assert.Equal(testData.LyricResultData.Replace("\r\n", "\n"), searchResult.LyricText.Replace("\r\n", "\n"));
        }

        [Theory]
        [InlineData("asdfasdfasdfasdf", "asdfasdfasdfasdf")]
        public void SearchLyric_NotExistsLyrics_ShouldReturnNoDataFoundStatus(string artist, string song)
        {
            // Arrange
            var lyricsClient = new AZLyricsProvider();
            var searchRequest = new ArtistAndSongSearchRequest(artist, song);
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var searchResult = lyricsClient.SearchLyric(searchRequest, cancellationToken);

            // Assert
            Assert.NotNull(searchResult);
            Assert.True(searchResult.IsEmpty());
            Assert.Equal(ResponseStatusCode.NoDataFound, searchResult.ResponseStatusCode);
            Assert.Equal(ExternalProviderType.AZLyrics, searchResult.ExternalProviderType);
            Assert.True(string.IsNullOrEmpty(searchResult.ResponseMessage));
            Assert.False(searchResult.Instrumental);
        }
    }
}
