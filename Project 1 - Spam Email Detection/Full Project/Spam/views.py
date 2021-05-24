from django.shortcuts import render
from django.http import HttpResponse
import os
import joblib

model1 = joblib.load(os.path.dirname(__file__) + "\\modelpipeline.pkl")


# Create your views here.
def index(request):
    return render(request, 'index.html')

def checkSpam(request):
    if(request.method == "POST"):
        finalAns = None
        algo = request.POST.get("algo")
        rawData = request.POST.get("rawdata")
        
        if(algo == "Algo-1"):
            finalAns = model1.predict([rawData])[0]
            param = {"answer" : finalAns}

        return render(request, 'output.html', param)
    else:
        return render(request, 'index.html')

    