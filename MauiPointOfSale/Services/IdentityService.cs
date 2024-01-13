using System.Text;
using System.Text.Json;

namespace MauiPointOfSale.Services
{
    public class IdentityService
    {
        private IHttpClientFactory _clientFactory;
        public HttpClient VerifyClient { get; set; }
        public HttpClient CloudClient { get; set; }
        public IdentityService(IHttpClientFactory client)
        {
            this._clientFactory = client;
            VerifyClient = _clientFactory.CreateClient("MicroBlinkVerify");
            CloudClient = _clientFactory.CreateClient("MicroBlinkCloud");
        }

        public async Task<IDBarcodeEndpointResponse> ExtractVoterData(IdBarcodeRequest content, string url)
        {
            //Serialize Object
            string json = JsonSerializer.Serialize(content);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Execute POST request
            HttpResponseMessage response = await CloudClient.PostAsync(url, jsonContent);
            string responseJson;
            if (response.IsSuccessStatusCode)
            {
                responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<IDBarcodeEndpointResponse>(responseJson);
                return result;
            }

            throw new Exception($"Call was unsuccessful. StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase}");
        }

        public async Task<IDBarcodeEndpointResponse> VerifyVoter(IdBarcodeRequest content, string url)
        {
            //Serialize Object
            string json = JsonSerializer.Serialize(content);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Execute POST request
            HttpResponseMessage response = await VerifyClient.PostAsync(url, jsonContent);
            string responseJson;
            if (response.IsSuccessStatusCode)
            {
                responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<IDBarcodeEndpointResponse>(responseJson);
                return result;
            }

            throw new Exception($"Call was unsuccessful. StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase}");
        }




    }

    public class IdBarcodeRequest
    {
        public string imageSource { get; set; }
        public string inputString { get; set; }
        public Int32 ageLimit { get; set; }
    }

    public class IDBarcodeEndpointResponse
    {
        public string executionId { get; set; }
        public string finishTime { get; set; }
        public string startTime { get; set; }
        public IDBarcodeRecognizerResult result { get; set; }
    }

    public class IDBarcodeRecognizerResult
    {
        public DateOfBirth dateOfBirth { get; set; }
        public ClassInfo classInfo { get; set; }
        public string type { get; set; }
        public bool? isBelowAgeLimit { get; set; }
        public int age { get; set; }
        public string recognitionStatus { get; set; }
        public string documentType { get; set; }
        public string rawDataBase64 { get; set; }
        public string stringData { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public string fullName { get; set; }
        public string additionalNameInformation { get; set; }
        public string address { get; set; }
        public string placeOfBirth { get; set; }
        public string nationality { get; set; }
        public string race { get; set; }
        public string religion { get; set; }
        public string profession { get; set; }
        public string maritalStatus { get; set; }
        public string residentialStatus { get; set; }
        public string employer { get; set; }
        public string sex { get; set; }
        public DateOfIssue dateOfIssue { get; set; }
        public DateOfExpiry dateOfExpiry { get; set; }
        public string documentNumber { get; set; }
        public string personalIdNumber { get; set; }
        public string documentAdditionalNumber { get; set; }
        public string issuingAuthority { get; set; }
        public string restrictions { get; set; }
        public string endorsements { get; set; }
        public string vehicleClass { get; set; }
        public string street { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string jurisdiction { get; set; }
        public List<ExtendedElement> extendedElements { get; set; }
    }
    public class ExtendedElement
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    public class DateOfBirth
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public bool successfullyParsed { get; set; }
        public string originalString { get; set; }
    }
    public class ClassInfo
    {
        public string country { get; set; }
        public string region { get; set; }
        public string type { get; set; }
        public string countryName { get; set; }
        public string isoAlpha3CountryCode { get; set; }
        public string isoAlpha2CountryCode { get; set; }
        public string isoNumericCountryCode { get; set; }
    }
    public class DateOfExpiry
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public bool successfullyParsed { get; set; }
        public string originalString { get; set; }
    }
    public class DateOfIssue
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public bool successfullyParsed { get; set; }
        public string originalString { get; set; }
    }
}
