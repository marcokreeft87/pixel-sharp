# PixelSharp

![pixelsharp](https://user-images.githubusercontent.com/10223677/235302181-3d1b693b-c611-4f31-b731-b0231c2d91fc.PNG)


## Hardware
1. A Raspberry Pi
2. A Pixel Display. I used [this display](https://www.amazon.nl/dp/B0B3GQD3JM?ref=ppx_yo2ov_dt_b_product_details&th=1)
3. A way to connect the display to the Raspberry Pi. I used the rainbow cable to jumper wires that came with the display and follow this mapping.
The colors of the columns are the colors of the rainbow cable.

![Untitled](https://user-images.githubusercontent.com/10223677/234552571-4b43de64-0d37-49ad-addc-ccd331c9f193.png)

## Software
1. Install [rpi-rgb-led-matrix](https://github.com/hzeller/rpi-rgb-led-matrix/tree/master)
2. Install .NET 7 with the following command
```
wget -O - https://raw.githubusercontent.com/pjgpetecodes/dotnet7pi/main/install.sh | sudo bash
```


## TODO
- [x] Support for gif
- [ ] Support for video
- [x] Support for text
- [x] Boot screen
- [ ] Clock
- [ ] Way to send whole screen

```
[{
  start: { 
    x: 10,
    y: 20
  },
  end: {
    x: 30,
    y: 50
  },
  graphic: {
    type: text,
    content: 'Hello'
  }
},
{

  start: { 
    x: 10,
    y: 20
  },
  end: {
    x: 30,
    y: 50
  },
  graphic: {
    type: image,
    content: 'http://'
  }
}]

```
