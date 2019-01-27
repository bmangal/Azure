using Newtonsoft.Json;

namespace CosmosDbDemo
{
    /// <summary>
    /// Strongly Typed object representing a single document.
    /// </summary>
	public class Person
	{
        /// <summary>
        /// Json property name is actual Cosmos DB property name in camelCase.
        /// </summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

        /// <summary>
        /// State is the Partition key
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "zipcode")]
        public string Zipcode { get; set; }

    }

}
