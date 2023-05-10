# PixelSharp

![pixelsharp](https://user-images.githubusercontent.com/10223677/235302181-3d1b693b-c611-4f31-b731-b0231c2d91fc.PNG)


## Hardware
1. A Raspberry Pi
2. A Pixel Display. I used [this display](https://www.amazon.nl/dp/B0B3GQD3JM?ref=ppx_yo2ov_dt_b_product_details&th=1)
3. A way to connect the display to the Raspberry Pi. I used the rainbow cable to jumper wires that came with the display and follow this mapping.
The colors of the columns are the colors of the rainbow cable.

![Untitled](https://user-images.githubusercontent.com/10223677/234552571-4b43de64-0d37-49ad-addc-ccd331c9f193.png)

## Software
1. Install .NET 7 with the following command
```
wget -O - https://raw.githubusercontent.com/pjgpetecodes/dotnet7pi/main/install.sh | sudo bash
```
2. Clone this project in the folder you want
3. Run the following command to build an run the project from [Your chosen folder]/src
```
sudo dotnet run --urls="http://*:5000"
```

## Troubleshooting
If you get the following error
![image](https://user-images.githubusercontent.com/10223677/236404600-54339297-fe1c-4ae9-9478-cb256b3e1457.png)

Follow these steps:
```
cd ../..
cd /etc/modprobe.d
sudo nano raspi-blacklist.conf
```
And type the following in the editor
```
blacklist snd_bcm2835
```
Exit and save changes.
Now you will need to reboot the pi and it should be working for you.

## Usage

### Render image on display
http://[Raspberry PI]:5000/matrix/image?imageUrl=[URL to image]

### Render gif on display
http://[Raspberry PI]:5000/matrix/gif?imageUrl=[URL to image]

### Render text on display
http://[Raspberry PI]:5000/matrix/text?text=hello

## Full pixel control
http://[Raspberry PI]:5000/matrix/render

With POST body:
```json
{
    "Width": 64,
    "Height": 64,
    "Sections": [
        {
            "Start": {
                "X": 1,
                "Y": 10
            },
            "End": {
                "X": 64,
                "Y": 20
            },
            "Graphic": {
                "Type": 0,
                "Content": "Itsa me"
            }
        },
        {
            "Start": {
                "X": 1,
                "Y": 18
            },
            "End": {
                "X": 64,
                "Y": 18
            },
            "Graphic": {
                "Type": 0,
                "Content": "MARIEKE!!"
            }
        },
        {
            "Start": {
                "X": 1,
                "Y": 28
            },
            "End": {
                "X": 32,
                "Y": 64
            },
            "Graphic": {
                "Type": 1,
                "Content": "https://i.gifer.com/origin/dd/dd77ec648bca7d408dc59a1984f533cf_w200.webp"
            }
        },
        {
            "Start": {
                "X": 33,
                "Y": 28
            },
            "End": {
                "X": 64,
                "Y": 64
            },
            "Graphic": {
                "Type": 1,
                "Content": "https://mario.wiki.gallery/images/3/3e/MPSS_Mario.png"
            }
        }
    ]
}
```


## TODO
- [x] Support for gif
- [x] Support for text
- [x] Boot screen
- [ ] App in MAUI
- [ ] Clock
- [ ] Weather
- [ ] RenderRequest lowercase
- [x] Pass pixels in a render section
- [ ] Gif support for render section
- [x] Add image/gif base64 support for render and image endpoint
- [ ] Back to default state
