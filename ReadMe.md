# Spatial Utilities for .NET applications

This library contains various spatial utilities to help any .NET application.

![](http://i.imgur.com/FGnyWDH.png)

![](https://ci.appveyor.com/api/projects/status/4klu6n9qeso35s3g)

## Api Services
### Nominatum Api Service
Returns some basic geospatial information from the [Nominatum web service](http://wiki.openstreetmap.org/wiki/Nominatim).

    // Arrange.
    var service  = new NominatumApiService();
    service.Email = "me@foo.com"; // Optional: they like this if you are doing lots of hits.
    service.Limit = 1; // Optional: how many results you would like.
    service.RestClient = mockRestClient; // Optional: you can pass in a mocked/fake rest client :)
    
    // Act.
    // Note: we need to cast, because the result type is a object.
    var result = service.Geocode("Bondi Beach, Sydney, Australia") as NominatumResponse;

    // Now you have access to various Nominatum specific data, like Latitude/Longitude and 
    // a verbose location/address.

### Google Maps Api Service
Returns some basic geospatial information from the [Google Api web service](https://developers.google.com/maps/documentation/webservices/).

    // Arrange.
    var service  = new GoogleMapsApiService();
    service.RestClient = mockRestClient; // Optional: you can pass in a mocked/fake rest client :)
    
    // Act.
    // Note: we need to cast, because the result type is a object.
    var result = service.Geocode("Bondi Beach, Sydney, Australia") as GoogleMapsResponse;
    result.

    // Now you have access to various Google Maps Api specific data, like Latitude/Longitude and 
    // a verbose location/address.
    
### Geocode 
Given an query/address, this get's the Latitude and Longitude of the location.

It uses the following 3rd party api's to resolve this (in order)    

  1. Nominatum
  2. Google Api.

.   // Arrange.
    var service = new GeocodingService();
    // Alternative way: var service = new GeocodingService( -a list of IApiServices-); // eg. GoogleMaps, your own, etc.
    
    // Act.
    var coordinate = service.Geocode("Bondi Beach, Sydney, Australia");
    
    // Now you can access
    // coordinate.Latitude
    // coordinate.Longitude

    Remarks: Learn what [geocoding is on Wikipedia(http://en.wikipedia.org/wiki/Geocoding).

License: this code is licensed under MIT.
-- end of file --
