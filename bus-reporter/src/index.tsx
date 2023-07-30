import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import L from 'leaflet';
/*
const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();*/

// Function to convert degrees to radians
function toRadians(degrees: number) {
  return degrees * (Math.PI / 180);
}

// Function to convert radians to degrees
function toDegrees(radians: number) {
  return radians * (180 / Math.PI);
}

// Function to calculate the distance between two points in kilometers
function haversineDistance(from: L.LatLng, to: L.LatLng) {
  const R = 6371; // Earth's radius in kilometers

  // Convert latitude and longitude from degrees to radians
  const φ1 = toRadians(from.lat);
  const φ2 = toRadians(to.lat);
  const Δφ = toRadians(to.lat - from.lat);
  const Δλ = toRadians(to.lng - from.lng);

  // Calculate haversine of differences
  const a = Math.sin(Δφ / 2) * Math.sin(Δφ / 2) +
            Math.cos(φ1) * Math.cos(φ2) *
            Math.sin(Δλ / 2) * Math.sin(Δλ / 2);

  // Calculate central angle using atan2
  const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

  // Calculate the distance in kilometers
  const d = R * c;
  return d;
}

function interpolateCoordinates(from: L.LatLng, to: L.LatLng, fraction: number) {
  const R = 6371; // Earth's radius in kilometers

  // Convert latitudes and longitudes from degrees to radians
  const φ1 = toRadians(from.lat);
  const φ2 = toRadians(to.lat);
  const λ1 = toRadians(from.lng);
  const λ2 = toRadians(to.lng);

  // Calculate the distance between points A and B using the haversine formula
  const distanceAB = haversineDistance(from, to);

  // Calculate the latitude and longitude of the point at the specified fraction of the distance
  const φC = φ1 + fraction * (φ2 - φ1);
  const λC = λ1 + fraction * (λ2 - λ1);

  // Convert the latitude and longitude of point C back to degrees
  const latC = toDegrees(φC);
  const lonC = toDegrees(λC);

  return { latitude: latC, longitude: lonC };
}

const map = L.map('map').setView([58.9700, 5.7331], 13);
L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
}).addTo(map);

var pointA = new L.LatLng(58.96579493713187, 5.732423324456156);
var pointB = new L.LatLng(58.957873279784145, 5.739718932122752);
var pointC = new L.LatLng(58.95406669632196, 5.74203636142489);
var pointD = new L.LatLng(58.94636371954393, 5.744010467028793);
var pointList = [pointA, pointB, pointC, pointD];
var firstpolyline = new L.Polyline(pointList, {
    color: 'red',
    weight: 3,
    opacity: 0.5,
    smoothFactor: 1
});
firstpolyline.addTo(map);

const speedDistance = 0.01; // km
const speedInterval = 100; // ms

const marker = L.marker([pointA.lat, pointA.lng]).addTo(map);

function connect() {
  var ws = new WebSocket('ws://localhost:5077/v2/buses/location/ws');
  ws.onopen = function() {
    console.log('CONNECTED');
  };

  ws.onmessage = function(e) {
    console.log(e.data);
    const msg = JSON.parse(e.data);
    // todo it should be smart to smooth noticeable intervals between location updates
    marker.setLatLng([msg.lat, msg.lon]);
  };

  ws.onclose = function(e) {
    console.log('Socket is closed. Reconnect will be attempted in 1 second.', e.reason);
    setTimeout(function() {
      connect();
    }, 1000);
  };

  ws.onerror = function(event) {
    console.log(event);
    console.error('Socket encountered error: ', event, 'Closing socket');
    ws.close();
  };
}

connect();
