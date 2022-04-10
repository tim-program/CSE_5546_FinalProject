import numpy as np
import torch
import torch.nn as nn
import torchvision.models as TVM
from torch.utils.data import DataLoader
from catch_dataset import CATCHDataset


class WSIImageDataset(torch.utils.data.Dataset):
    pass


class InterestSpotNetwork(nn.Module):
    def __init__(self, num_classes: int = 2) -> None:
        super().__init__()
        self.backbone = TVM.detection.fasterrcnn_resnet50_fpn(pretrained=True)
        in_features = self.backbone.roi_heads.box_predictor.cls_score.in_features
        # replace the pre-trained head with a new one
        self.backbone.roi_heads.box_predictor = TVM.detection.faster_rcnn.FastRCNNPredictor(
            in_features, num_classes)

    def forward(self, data):
        return self.backbone(data)


if __name__ == "__main__":
    ntwk = InterestSpotNetwork()
    ntwk.eval()

    catch_ds = CATCHDataset(
        "C:\\Users\\bunch\\CSE 5546 Labs\\CSE_5546_FinalProject\\InterestSpotNetwork\\traintestval_pipeline\\data\\",
        relevant_categories=["Melanoma"],
        crop_size=250)
    catch_dl = DataLoader(catch_ds, shuffle=True)
    print("Initialized Dataloader. Starting Inference.")

    for batch, labels in catch_dl:
        print(ntwk([batch[i] for i in range(batch.shape[0])]))
        # print("-" * 1000)
        # print(batch)
        # print(labels)
        # print("-" * 1000)

    # import tifffile
    # ntwk = InterestSpotNetwork()
    # ntwk.eval()
    # print("Loading Image")
    # img = tifffile.imread(
    #     "C:\\Users\\bunch\\CSE 5546 Labs\\CSE_5546_FinalProject\\InterestSpotNetwork\\traintestval_pipeline\\data\\images\\Melanoma_01_1.tiff")
    # print(f"\tImage Shape: {img.shape}")
    # print("\tCropping Image")
    # cx, cy = int(img.shape[0] / 2), int(img.shape[1] / 2)
    # crop_size = 250
    # x, y = cx - int(crop_size / 2), cy - int(crop_size / 2)
    # img = img[x:x + crop_size, y:y + crop_size]
    # print("\tScaling the Image")
    # img = (img - np.amin(img, axis=(0, 1))) / np.amax(img, axis=(0, 1))
    # img = img.transpose((2, 0, 1))
    # print("\tTorching the Image")
    # data = torch.from_numpy(img).float()
    # print(f"\tImage Shape: {img.shape}")
    # print("Image Loaded. Running Inference")
    # print(ntwk([data]))
    # print("Inference Done")
