var xArray = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
var yArray = [7, 15, 10, 9, 12, 20, 21, 11, 14, 14, 15, 15];

// Define Data
var data = [{
    x: xArray,
    y: yArray,
    mode: "line"
}];

// Define Layout
var layout = {
    xaxis: { range: [0, 12], title: "Aylar" },
    yaxis: { range: [0, 50], title: "Kiralama Sayıları" },
    title: "Aylık Oto Kiralama Sayıları"
};

// Display using Plotly
Plotly.newPlot("myPlot", data, layout);