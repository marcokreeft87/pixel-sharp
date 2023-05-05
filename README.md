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
3. Clone this project in the folder you want
4. Run the following command to build an run the project
```
sudo dotnet run [Your chosen folder]/src --urls="http://*:5000"
```

## Troubleshooting
If you get the following error
![image](https://user-images.githubusercontent.com/10223677/236404600-54339297-fe1c-4ae9-9478-cb256b3e1457.png)

Follow these steps:
``
cd ../..
cd /etc/modprobe.d
sudo nano raspi-blacklist.conf
``
And type the following in the editor
```
blacklist snd_bcm2835
```
Exit and save changes.
Now you will need to reboot the pi and it should be working for you.

## TODO
- [x] Support for gif
- [x] Support for text
- [x] Boot screen
- [ ] Clock
- [ ] Weather
- [ ] RenderRequest lowercase
- [x] Pass pixels in a render section
- [ ] Gif support for render section
- [ ] Add image/gif base64 support for render and image endpoint
- [ ] Back to default state
