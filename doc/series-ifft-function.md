---
title: series_ifft() - Azure Data Explorer
description: Learn how to use the series_ifft() function to apply the Inverse Fast Fourier Transform (IFFT) on a series.
ms.reviewer: adieldar
ms.topic: reference
ms.date: 01/29/2023
---
# series_ifft()

Applies the Inverse Fast Fourier Transform (IFFT) on a series.  

The series_ifft() function takes a series of complex numbers in the frequency domain and transforms it back to the time/spatial domain using the [Fast Fourier Transform](https://en.wikipedia.org/wiki/Fast_Fourier_transform). This function is the complementary function of [series_fft](series-fft-function.md). Commonly the original series is transformed to the frequency domain for spectral processing and then back to the time/spatial domain.

## Syntax

`series_ifft(`*fft_real* [`,` *fft_imaginary*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *fft_real* | dynamic | &check; | An array of numeric values representing the real component of the series to transform.|
| *fft_imaginary* | dynamic | | An array of numeric values representing the imaginary component of the series. This parameter should be specified only if the input series contains complex numbers.|

## Returns

The function returns the complex inverse fft in two series. The first series for the real component and the second one for the imaginary component.

## Example

See [series_fft](series-fft-function.md#example)
