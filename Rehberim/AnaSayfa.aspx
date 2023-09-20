<%@ Page Title="Ana Sayfa" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnaSayfa.aspx.cs" Inherits="Rehberim._AnaSayfa" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
            background-image: url('Images/anasayfa2.jpg'); 
            background-size: cover;
            background-repeat: no-repeat;
            background-attachment: fixed;
        }

        .content-container {
            display: flex;
            align-items: center;
        }

        .welcome-text {
            flex:none;
            font-size: 50px;
            padding: 20px;
            color: darkseagreen;
            margin-top: 10px;
            font-family: 'Brush Script MT', sans-serif;
        }       

        .lead {
            font-size: 20px; 
        }

        .weather-container {
            
            margin-top: 50px;
            color:darkseagreen;
        }

       

        footer {
            padding: 5px; 
            position: fixed;
            bottom: 0;
            width: 100%;
            
        }
    </style>

    <main>
        <section class="row" aria-labelledby="welcomeTitle">
            <div class="content-container">
                <div class="welcome-text">
                    <h1 id="welcomeTitle" style="margin-bottom: 35px; font-family: 'Brush Script MT', sans-serif; font-size: 100px; color: black;">Hoşgeldin</h1>
                    <p class="lead" style="font-size: 50px;"><strong><span style="font-weight: 700;">Bugünün tarihi: </span></strong><span id="tarih" style="font-weight:500;"></span></p>
                    <p class="lead" style="font-size: 50px;"><strong><span style="font-weight: 700;">Saat: </span></strong><span id="saat" style="font-weight: 500;"></span></p>

                    
                    <div class="weather-container">
                        <p class="weather-description" style="font-size: 50px;"><strong><span style="font-weight: 700;" >Hava Durumu: </span></strong><span id="weatherDesc" style="font-weight:500;"></span></p>
                        <p class="weather-temperature" style="font-size: 50px;"><strong><span style="font-weight: 700;" >Sıcaklık: </span></strong><span id="temperature" style="font-weight:500;"></span>°C</p>
                    </div>
                </div>


            </div>
        </section>
    </main>

    <script>
        function updateDateTime() {
            var now = new Date();

            var tarihElement = document.getElementById("tarih");
            tarihElement.textContent = now.toLocaleDateString('tr-TR', {
                day: 'numeric',
                month: 'long',
                year: 'numeric'
            });

            var saatElement = document.getElementById("saat");
            saatElement.textContent = now.toLocaleTimeString('tr-TR');
        }

        function updateWeather() {
            var apiKey = '73d6edd78484a9d1a088732d1a30a493';
            var city = 'Ankara'; 

            var apiUrl = `https://api.openweathermap.org/data/2.5/weather?q=${city}&units=metric&lang=tr&appid=73d6edd78484a9d1a088732d1a30a493`;

            fetch(apiUrl)
                .then(response => response.json())
                .then(data => {
                    var weatherDesc = data.weather[0].description;
                    var temperature = data.main.temp;

                    document.getElementById("weatherDesc").textContent = weatherDesc;
                    document.getElementById("temperature").textContent = temperature;
                })
                .catch(error => {
                    console.error('Hava durumu verisi alınırken hata oluştu:', error);
                });
        }

        
        window.onload = function () {
            updateDateTime(); // 
            setInterval(updateDateTime, 1000); 

            updateWeather(); 
            setInterval(updateWeather, 600000); 

            
            var kullaniciAdi = '<%= Session["UserID"] %>';
            document.getElementById("welcomeTitle").textContent = "Hoşgeldin " + kullaniciAdi;
        };
    </script>


</asp:Content>
