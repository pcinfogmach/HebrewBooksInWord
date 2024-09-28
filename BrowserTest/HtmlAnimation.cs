﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserTest
{
    public static class HtmlAnimation
    {
        public static string LoadingInProgress()
        {
            return @"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Loading Animation</title>
  <style>
    body {
      display: flex;
      justify-content: center;
      align-items: center;
      height: 100vh;
      background-color: #f0f0f0;
      margin: 0;
    }
    
    .loading-container {
      display: flex;
      justify-content: space-between;
      width: 60px;
    }
    
    .dot {
      width: 15px;
      height: 15px;
      background-color: #3498db;
      border-radius: 50%;
      animation: bounce 0.6s infinite alternate;
    }
    
    .dot:nth-child(2) {
      animation-delay: 0.2s;
    }
    
    .dot:nth-child(3) {
      animation-delay: 0.4s;
    }
    
    @keyframes bounce {
      to {
        transform: translateY(-15px);
      }
    }
  </style>
</head>
<body>
  <div class=""loading-container"">
    <div class=""dot""></div>
    <div class=""dot""></div>
    <div class=""dot""></div>
  </div>

  <script>
    // You can add any JavaScript here if needed.
  </script>
</body>
</html>
";
        }

    }
}
