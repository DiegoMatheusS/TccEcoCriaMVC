
function buscarCoordenadasPorCEP(cep, callback) {
  var geocoder = new google.maps.Geocoder();
  geocoder.geocode({ address: cep }, function (results, status) {
      if (status === "OK") {
          var location = results[0].geometry.location;
          callback(location);

          // Remove o marcador anterior se existir
          if (centralMarker) {
              centralMarker.setMap(null);
          }

          // Adiciona o marcador vermelho sobre o ponto central
          centralMarker = new google.maps.Marker({
              map: map,
              position: location,
              icon: {
                  url: "http://maps.google.com/mapfiles/ms/icons/red-dot.png"
              }
          });
      } else {
          alert("Não foi possível encontrar o CEP: " + status);
          callback(null);
      }
  });
}

function initMap() {
  var defaultCenter = { lat: -23.55052, lng: -46.633308 };
  map = new google.maps.Map(document.getElementById("map"), {
    zoom: 12,
    center: defaultCenter,
  });

  document.querySelector("form").addEventListener("submit", function (e) {
    e.preventDefault();
    var cep = document.getElementById("cep").value;
    var distance = document.getElementById("distance").value;
    var produto = document.getElementById("produto").value;

    if (cep && distance && produto) {
      buscarCoordenadasPorCEP(cep, function (location) {
        if (location) {
          var distanceInMeters = parseInt(distance) * 1000;
          buscarPontosDeColeta(map, location, distanceInMeters, produto);

          if (currentCircle) {
            currentCircle.setMap(null);
          }

          currentCircle = new google.maps.Circle({
            map: map,
            radius: distanceInMeters,
            fillColor: '#60c659',
            fillOpacity: 0,
            strokeColor: '#60c659',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            center: location,
          });
        }
      });
    }
  });
}

let map;
let currentCircle = null;
let centralMarker = null;
let markers = [];

function clearMarkers() {
  for (var i = 0; i < markers.length; i++) {
    markers[i].setMap(null);
  }
  markers = [];
}

function buscarPontosDeColeta(map, location, distance, produto) {
  clearMarkers();
  var service = new google.maps.places.PlacesService(map);

  var request = {
    location: location,
    radius: distance,
    query: produto,
  };

  service.textSearch(request, function (results, status) {
    if (status === google.maps.places.PlacesServiceStatus.OK) {
      for (var i = 0; i < results.length; i++) {
        var place = results[i];

        var distanceInMeters = google.maps.geometry.spherical.computeDistanceBetween(location, place.geometry.location);

        if (distanceInMeters <= distance) {
          createGreenMarker(map, place);
        }
      }
      map.setCenter(location);
    }
  });
}

function createGreenMarker(map, place) {
  var marker = new google.maps.Marker({
    map: map,
    position: place.geometry.location,
    icon: {
      url: "http://maps.google.com/mapfiles/ms/icons/green-dot.png",
    //url: "~/img/logo.ponto.png", // Substitua com a URL da imagem desejada
    //scaledSize: new google.maps.Size(32, 32) // Define o tamanho do ícone, ajustável conforme necessário
    },
  });
  markers.push(marker);
}


//Função para formatação do cep no card.
function formatarCEP(input) {
  // Remove caracteres não numéricos
  let valor = input.value.replace(/\D/g, '');

  // Adiciona o traço automaticamente
  if (valor.length > 5) {
    valor = valor.slice(0, 5) + '-' + valor.slice(5);
  }

  // Atualiza o valor do input
  input.value = valor;
}