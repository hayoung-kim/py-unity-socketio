import eventlet
eventlet.monkey_patch(socket=True, select=True, time=True)

import socketio
from flask import Flask, render_template

print(" [-] package loaded")

sio = socketio.Server()
app = Flask(__name__)
msgs = []
target = 7.7

@sio.on('connect')
def connect(sid, environ):
    print("connect ", sid)

@sio.on('test')
def test(sid, data):
    y = float(data["y"])
    u = -1.2 * (y - target)
    sio.emit('speed', data={'vy': str(u)})

    print("y = ", float(data["y"]))
    print("u = ", u)
    print("--------------------")

@sio.on('disconnect')
def disconnect(sid):
    print('disconnect ', sid)

if __name__ == '__main__':

    # wrap Flask application with engineio's middleware
    app = socketio.Middleware(sio, app)

    # deploy as an eventlet WSGI server
    eventlet.wsgi.server(eventlet.listen(('', 4473)), app)
