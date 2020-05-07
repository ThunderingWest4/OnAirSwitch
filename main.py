import requests as rq
from tkinter import *

window = Tk()
device_id = "23003d001847343438323536"
device_token = "90f13806556687c42a0b9742f0fab56356d909db"
function_name = "Toggle"
URL = "https://api.particle.io/v1/devices/" + device_id + "/" + function_name

def onClick():
    print("light toggled")
    rq.post(URL, data={'args':"go", 'access_token':device_token})

b = Button(window, text="Initiate On-Air Light", command=onClick)
b.pack()

mainloop()

