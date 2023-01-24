#!/usr/bin/env python
from importlib import import_module
import os
import cv2
from flask import Flask, render_template, Response, request
from sqlConnector import SqlConnector

app = Flask(__name__)

@app.route('/')
def index():
    """Contoso Supermarket home page."""
    cameras_enabled = True
    if os.environ.get('CAMERAS_ENABLED'):
        cameras_enabled = os.environ.get('CAMERAS_ENABLED') == 'True'
    
    head_title = "Contoso Supermarket"
    if os.environ.get('HEAD_TITLE'):
        head_title = os.environ.get('HEAD_TITLE')

    new_category = False
    if os.environ.get('NEW_CATEGORY'):
        new_category = os.environ.get('NEW_CATEGORY') == 'True'

    return render_template('index2.html' if new_category else 'index.html', head_title = head_title, cameras_enabled = cameras_enabled)

@app.route('/addPurchase',methods = ['POST'])
def addPurchase():
    content_type = request.headers.get('Content-Type')
    if (content_type == 'application/json; charset=UTF-8'):
        json = request.get_json()
        sqlDb = SqlConnector()
        successful = sqlDb.addPurchase(json['ProductId'])
        if(successful):
            return "Ok"
        else:
            return "Error processing request"
    else:
        return 'Content-Type not supported!'

@app.route('/video_feed/<feed>')
def video_feed(feed):
    return Response(gen_frames(feed),
                    mimetype='multipart/x-mixed-replace; boundary=frame')


def gen_frames(source):
    """Video streaming frame capture function."""
    baseUrl = "rtsp://localhost:554/media/" 
    if os.environ.get('CAMERAS_BASEURL'):
        baseUrl = str(os.environ['CAMERAS_BASEURL'])

    cap = cv2.VideoCapture(baseUrl + source)  # capture the video from the live feed

    while True:
        # # Capture frame-by-frame. Return boolean(True=frame read correctly. )
        success, frame = cap.read()  # read the camera frame
        if not success:
            break
        else:
            ret, buffer = cv2.imencode('.jpg', frame)
            frame = buffer.tobytes()
            yield (b'--frame\r\n'
                   b'Content-Type: image/jpeg\r\n\r\n' + frame + b'\r\n')  # concat frame one by one and show result

if __name__ == '__main__':
    app.run(host='0.0.0.0', threaded=True, debug=True)
