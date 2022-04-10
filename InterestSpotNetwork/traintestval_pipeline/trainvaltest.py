from datetime import date
import sys
import os

sys.path.append(os.path.abspath("../"))
print(f"Last Value of Path: {sys.path[-1]}")

import torch
from torch import optim
from torch.utils.data import DataLoader
import engine
import utils

from model_def import InterestSpotNetwork
from catch_dataset import CATCHDataset

doTrain = True

device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
print(f"Device: {device}")

print("Loading CATCH Dataset")
catch_ds = CATCHDataset(
    "C:\\Users\\bunch\\CSE 5546 Labs\\CSE_5546_FinalProject\\InterestSpotNetwork\\traintestval_pipeline\\data\\",
    relevant_categories=["Melanoma"],
    crop_size=250)
catch_dl = DataLoader(catch_ds, shuffle=True)

print("Initializing Network for Training")
isn = InterestSpotNetwork()
isn.to(device)
params = [p for p in isn.parameters() if p.requires_grad]
optimizer = optim.SGD(params, lr=0.005, momentum=0.9, weight_decay=0.0005)
lr_scheduler = optim.lr_scheduler.StepLR(optimizer, step_size=3, gamma=0.1)

if doTrain:
    print("Starting Training")
    isn.train()
    epochs = 10
    for i in range(epochs):
        engine.train_one_epoch(isn, optimizer, catch_dl,
                               device, i, print_freq=100)
        lr_scheduler.step()
        engine.evaluate(isn, catch_dl, device)

fn = date.today().strftime("%Y.%m.%d.%H.%M.pnn")
torch.save(isn.state_dict(), f"../cnn_server/{fn}.pt")
