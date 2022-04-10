import os
import numpy as np
import torch
import torch.utils.data
import torchvision.transforms as transforms
import json
import tifffile


class CATCHDataset(torch.utils.data.Dataset):
    def __init__(self, root, relevant_categories=None, crop_size=250):
        self.root = root
        self.crop_size = crop_size
        # self.transforms = transforms
        # load all image files, sorting them to
        # ensure that they are aligned
        with open(os.path.join(root, "CATCH.json"), 'rt') as f:
            self.data = json.load(f)
            self.data['categories'] = [cat for cat in self.data['categories']
                                       if cat['id'] == 7]
            self.data['images'] = [img for img in self.data['images']
                                   if "Melanoma" in img['file_name']]
            # for i, annot in enumerate(self.data['annotations']):
            #     self.data['annotations'][i] = {
            #         k: v for k, v in annot.items() if "segmentation" not in k}
            ids = [img['id'] for img in self.data['images']]
            self.data['annotations'] = [annot for annot in self.data['annotations']
                                        if annot['image_id'] in ids and annot['category_id'] == 7]

    def __getitem__(self, idx):
        # load images ad masks
        img = self.data['images'][idx]
        img_path = os.path.join(self.root, "images", img['file_name'])
        img_data = torch.from_numpy(tifffile.imread(img_path))
        x1, x2, y1, y2 = self.__center_crop(img_data, crop_size=self.crop_size)
        img_data = img_data[x1:x2, y1:y2]
        img_data = img_data.permute(dims=(2, 0, 1)).float()

        annots = [annot for annot in self.data['annotations']
                  if annot['image_id'] == img['id']]
        masks = []
        # get bounding box coordinates for each mask
        num_objs = 0
        boxes = []
        for annot in annots:
            mask = np.zeros_like(img_data)
            valid_xs = []
            valid_ys = []
            for i in range(len(annot['segmentation']) - 1):
                x, y = annot['segmentation'][i], annot['segmentation'][i + 1]
                if x in range(x1, x2) and y in range(y1, y2):
                    valid_xs.append(x)
                    valid_ys.append(y)
            mask[valid_xs, valid_ys] = 1

            if(1 in np.unique(mask)):
                masks.append(mask)
                # masks[i, [int(j / 1000) for j in annot['segmentation'][0::2]],
                #       [int(j / 1000) for j in annot['segmentation'][1::2]]] = 1
                pos = np.where(mask)
                xmin = np.min(pos[1])
                xmax = np.max(pos[1])
                ymin = np.min(pos[0])
                ymax = np.max(pos[0])
                boxes.append([xmin, ymin, xmax, ymax])
                num_objs = num_objs + 1

        masks = torch.as_tensor(masks, dtype=torch.int)
        boxes = torch.as_tensor(boxes, dtype=torch.float32)
        # there is only one class
        labels = torch.ones((num_objs,), dtype=torch.int64)
        masks = torch.as_tensor(masks, dtype=torch.uint8)

        image_id = torch.tensor([idx])
        area = 0
        if len(boxes.shape) == 4:
            area = (boxes[:, 3] - boxes[:, 1]) * (boxes[:, 2] - boxes[:, 0])
        # suppose all instances are not crowd
        iscrowd = torch.zeros((num_objs,), dtype=torch.int64)

        target = {}
        target["boxes"] = boxes
        target["labels"] = labels
        target["masks"] = masks
        target["image_id"] = image_id
        target["area"] = area
        target["iscrowd"] = iscrowd

        # if self.transforms is not None:
        #     img_data = self.transforms(img_data)
        #     target = self.transforms(target)

        return img_data, target

    def __len__(self):
        return len(self.data)

    def __center_crop(self, img: np.ndarray, crop_size):
        if type(crop_size) == int:
            crop_size = (crop_size, crop_size)
        cx, cy = int(img.shape[0] / 2), int(img.shape[1] / 2)
        x, y = cx - int(crop_size[0] / 2), cy - int(crop_size[1] / 2)
        return x, x + crop_size[0], y, y + crop_size[1]


if __name__ == "__main__":
    catch_ds = CATCHDataset(
        "C:\\Users\\bunch\\CSE 5546 Labs\\CSE_5546_FinalProject\\InterestSpotNetwork\\traintestval_pipeline\\data\\",
        relevant_categories=["Melanoma"],
        transforms=transforms.Compose(
            [transforms.CenterCrop(250)]
        ))
    torch.utils.data.DataLoader(catch_ds)
