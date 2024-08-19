﻿using LyricsScraperNET.Models.Requests;
using LyricsScraperNET.Models.Responses;
using LyricsScraperNET.Providers.Models;
using LyricsScraperNET.Providers.Musixmatch;
using LyricsScraperNET.TestShared.Providers;
using LyricsScraperNET.UnitTest.TestModel;
using Xunit;

namespace LyricsScraperNET.IntegrationTest.Providers.Musixmatch
{
    public class MusixmatchProviderTest : ProviderTestBase
    {
        [Theory (Skip="Bug with the infinite requests loop. Should be enabled after https://github.com/skuill/LyricsScraperNET/issues/24")]
        [MemberData(nameof(GetTestData), parameters: "Providers\\Musixmatch\\lyric_test_data.json")]
        public void SearchLyric_IntegrationDynamicData_Success(LyricsTestData testData)
        {
            // Arrange
            var lyricsClient = new MusixmatchProvider();
            SearchRequest searchRequest = CreateSearchRequest(testData);

            // Act
            var searchResult = lyricsClient.SearchLyric(searchRequest);

            // Assert
            Assert.NotNull(searchResult);
            Assert.False(searchResult.IsEmpty());
            Assert.Equal(ResponseStatusCode.Success, searchResult.ResponseStatusCode);
            Assert.True(string.IsNullOrEmpty(searchResult.ResponseMessage));
            Assert.Equal(ExternalProviderType.Musixmatch, searchResult.ExternalProviderType);
            Assert.Equal(testData.LyricResultData.Replace("\r\n", "\n"), searchResult.LyricText);
            Assert.False(searchResult.Instrumental);
        }

        [Theory(Skip = "Bug with the infinite requests loop. Should be enabled after https://github.com/skuill/LyricsScraperNET/issues/24")]
        [MemberData(nameof(GetTestData), parameters: "Providers\\Musixmatch\\instrumental_test_data.json")]
        public void SearchLyric_IntegrationDynamicData_Instrumental(LyricsTestData testData)
        {
            // Arrange
            var lyricsClient = new MusixmatchProvider();
            SearchRequest searchRequest = CreateSearchRequest(testData);

            // Act
            var searchResult = lyricsClient.SearchLyric(searchRequest);

            // Assert
            Assert.NotNull(searchResult);
            Assert.True(searchResult.IsEmpty());
            Assert.Equal(ResponseStatusCode.Success, searchResult.ResponseStatusCode);
            Assert.True(string.IsNullOrEmpty(searchResult.ResponseMessage));
            Assert.Equal(ExternalProviderType.Musixmatch, searchResult.ExternalProviderType);
            Assert.True(searchResult.Instrumental);
        }
    }
}
