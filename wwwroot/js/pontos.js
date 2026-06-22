let map;
let currentCircle = null;
let centralMarker = null;
let markers = [];

// Inicia o mapa assim que a página carrega
document.addEventListener("DOMContentLoaded", function () {
  initMap();
});

function initMap() {
  // Coordenadas centrais padrão (São Paulo)
  var defaultCenter = [-23.55052, -46.633308];
  
  // Cria o mapa do Leaflet
  map = L.map('map').setView(defaultCenter, 12);

  // Adiciona a camada visual gratuita do OpenStreetMap
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap contributors'
  }).addTo(map);

  // Escuta o envio do formulário
  document.querySelector("form").addEventListener("submit", function (e) {
    e.preventDefault();
    var cep = document.getElementById("cep").value;
    var distance = document.getElementById("distance").value;
    var produto = document.getElementById("produto").value;

    if (cep && distance && produto) {
      buscarCoordenadasPorCEP(cep, function (location) {
        if (location) {
          var distanceInMeters = parseInt(distance) * 1000;
          buscarPontosDeColeta(location, distanceInMeters, produto);

          // Remove o círculo antigo se existir
          if (currentCircle) {
            map.removeLayer(currentCircle);
          }

          // Desenha o novo círculo de alcance verde
          currentCircle = L.circle(location, {
            color: '#60c659',
            fillColor: '#60c659',
            fillOpacity: 0.1,
            weight: 2,
            radius: distanceInMeters
          }).addTo(map);
          
          // Centraliza o mapa no novo ponto e ajusta o zoom
          map.setView(location, 13);
        }
      });
    }
  });
}

// === AQUI ESTÁ A CORREÇÃO DO VIACEP ===
async function buscarCoordenadasPorCEP(cep, callback) {
  try {
    let cepLimpo = cep.replace(/\D/g, '');
    
    // 1. Pergunta ao ViaCEP qual é a rua e a cidade desse CEP
    let viaCepResponse = await fetch(`https://viacep.com.br/ws/${cepLimpo}/json/`);
    let viaCepData = await viaCepResponse.json();

    if (viaCepData.erro) {
      alert("CEP não encontrado na base dos Correios.");
      callback(null);
      return;
    }

    // 2. Monta o endereço certinho para o mapa
    let endereçoParaBusca = `${viaCepData.logradouro}, ${viaCepData.localidade}, ${viaCepData.uf}, Brazil`;
    let query = encodeURIComponent(endereçoParaBusca);

    // 3. Pergunta ao Nominatim onde fica essa rua no mapa
    let mapResponse = await fetch(`https://nominatim.openstreetmap.org/search?q=${query}&format=json&limit=1`);
    let mapData = await mapResponse.json();

    if (mapData && mapData.length > 0) {
      var location = L.latLng(mapData[0].lat, mapData[0].lon);
      
      if (centralMarker) {
        map.removeLayer(centralMarker);
      }

      centralMarker = L.marker(location).addTo(map);
      centralMarker.bindPopup(`<b>Sua Localização</b><br>${viaCepData.logradouro}`).openPopup();

      callback(location);
    } else {
      alert("Endereço encontrado nos Correios, mas o mapa não conseguiu localizá-lo exato.");
      callback(null);
    }
  } catch (error) {
    console.error("Erro ao buscar CEP:", error);
    alert("Erro ao conectar com o serviço de buscas.");
    callback(null);
  }
}
// ========================================

function clearMarkers() {
  for (var i = 0; i < markers.length; i++) {
    map.removeLayer(markers[i]);
  }
  markers = [];
}

async function buscarPontosDeColeta(location, distance, produto) {
  clearMarkers();

  var resultList = document.getElementById("result-list");
  resultList.innerHTML = "";

  try {
    let query = encodeURIComponent(produto);
    let response = await fetch(`https://nominatim.openstreetmap.org/search?q=${query}&format=json&limit=10`);
    let results = await response.json();

    if (results.length > 0) {
      for (var i = 0; i < results.length; i++) {
        var place = results[i];
        var placeLocation = L.latLng(place.lat, place.lon);
        var distanceInMeters = map.distance(location, placeLocation);

        if (distanceInMeters <= distance) {
          createMarker(placeLocation, place.display_name);

          var listItem = document.createElement("div");
          listItem.className = "result-item";
          let nomeAbreviado = place.display_name.split(',')[0]; 
          listItem.innerHTML = `<strong>${nomeAbreviado}</strong><br>${place.display_name}`;
          resultList.appendChild(listItem);
        }
      }
    } else {
      var listItem = document.createElement("div");
      listItem.innerHTML = `<p style="padding: 15px;">Nenhum ponto público encontrado para "${produto}" nesta região.</p>`;
      resultList.appendChild(listItem);
    }
  } catch (error) {
    console.error("Erro ao buscar pontos:", error);
  }
}

function createMarker(location, enderecoCompleto) {
  var marker = L.marker(location).addTo(map);
  let titulo = enderecoCompleto.split(',')[0];
  marker.bindPopup(`<div><strong>${titulo}</strong><br>${enderecoCompleto}</div>`);
  markers.push(marker);
}

// Função para formatação do cep no card.
function formatarCEP(input) {
  let valor = input.value.replace(/\D/g, '');
  if (valor.length > 5) {
    valor = valor.slice(0, 5) + '-' + valor.slice(5);
  }
  input.value = valor;
}

// Função para voltar a página para cima
const toTop = document.querySelector(".to-top-ponto");
window.addEventListener("scroll", () => {
  if(window.scrollY > 1200){
    toTop.classList.add("active");
  } else {
    toTop.classList.remove("active");
  }
});