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

function buscarPontosDeColeta(map, location, distance, produto, types) {
  clearMarkers();

  // Limpa a lista de resultados anterior
  var resultList = document.getElementById("result-list");
  resultList.innerHTML = "";

  var service = new google.maps.places.PlacesService(map);

  var request = {
    location: location,
    radius: distance,
    query: produto,
    type: types
  };

  service.textSearch(request, function (results, status) {
    if (status === google.maps.places.PlacesServiceStatus.OK) {
      for (var i = 0; i < results.length; i++) {
        var place = results[i];

        // Calcular a distância entre o local encontrado e o centro do círculo
        var distanceInMeters = google.maps.geometry.spherical.computeDistanceBetween(location, place.geometry.location);

        if (distanceInMeters <= distance) {
          createMarker(map, place);

          // Adiciona o nome e o endereço à lista de resultados
          var listItem = document.createElement("div");
          listItem.className = "result-item";
          listItem.innerHTML = `<strong>${place.name}</strong><br>${place.formatted_address}`;
          resultList.appendChild(listItem);

        }
      }
      map.setCenter(location);
    }
  });
}



function createMarker(map, place) {
  var marker = new google.maps.Marker({
    map: map,
    position: place.geometry.location,
    icon: {
      url: "http://maps.google.com/mapfiles/ms/icons/green-dot.png",
    },
  });
  markers.push(marker);

  // Adiciona uma janela de informação ao clicar no marcador
  var infowindow = new google.maps.InfoWindow({
    content: `<div><strong>${place.name}</strong><br>${place.formatted_address}</div>`,
  });

  marker.addListener("click", function () {
    infowindow.open(map, marker);
  });
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

//Função para voltar a página para cima
const toTop = document.querySelector(".to-top-ponto");

window.addEventListener("scroll", () => {
    if(window.scrollY > 1200){
        toTop.classList.add("active");
    }
    else{
        toTop.classList.remove("active");
    }
})
