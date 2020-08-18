---
title: series_fft() - Azure Data Explorer
description: This article describes series_fft() function in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: adieldar
ms.service: data-explorer
ms.topic: reference
ms.date: 08/13/2020
---
# series_fft()

Applies the Fast Fourier Transform (FFT) on a series.  

The series_fft() function takes a series of complex numbers in the time/spatial domain and transforms it to the frequency domain using the [Fast Fourier Transform](https://en.wikipedia.org/wiki/Fast_Fourier_transform). The transformed complex series represents the magnitude and phase of the frequencies appearing in the original series. Use the complementary function [series_ifft](series-ifft-function.md) to transform from the frequency domain back to the time/spatial domain.

## Syntax

`series_fft(`*x_real* [`,` *x_imaginary*]`)`

## Arguments

* *x_real*: Dynamic array of numeric values representing the real component of the series to transform.
* *x_imaginary*: A similar dynamic array representing the imaginary component of the series. This parameter is optional and should be specified only if the input series contains complex numbers.

## Returns

The function returns the complex inverse fft in two series. The first series for the real component and the second one for the imaginary component.

## Example

* Generate a complex series, where the real and imaginary components are pure sine waves in different frequencies. Use FFT to transform it to the frequency domain:

    <!-- csl: https://help.kusto.windows.net:443/Samples -->
    ```kusto
    let sinewave=(x:double, period:double, gain:double=1.0, phase:double=0.0)
    {
        gain*sin(2*pi()/period*(x+phase))
    }
    ;
    let n=128;      //  signal length
    range x from 0 to n-1 step 1 | extend yr=sinewave(x, 8), yi=sinewave(x, 32)
    | summarize x=make_list(x), y_real=make_list(yr), y_imag=make_list(yi)
    | extend (fft_y_real, fft_y_imag) = series_fft(y_real, y_imag)
    | render linechart with(ysplit=panels)
    ```
    
    This query returns *fft_y_real* and *fft_y_imag*:  
    
    :::image type="content" source="images/series-fft-function/series-fft.png" alt-text="Series fft" border="false":::
    
* Transform a series to the frequency domain, and then apply the inverse transform to get back the original series:

    <!-- csl: https://help.kusto.windows.net:443/Samples -->
    ```kusto
    let sinewave=(x:double, period:double, gain:double=1.0, phase:double=0.0)
    {
        gain*sin(2*pi()/period*(x+phase))
    }
    ;
    let n=128;      //  signal length
    range x from 0 to n-1 step 1 | extend yr=sinewave(x, 8), yi=sinewave(x, 32)
    | summarize x=make_list(x), y_real=make_list(yr), y_imag=make_list(yi)
    | extend (fft_y_real, fft_y_imag) = series_fft(y_real, y_imag)
    | extend (y_real2, y_image2) = series_ifft(fft_y_real, fft_y_imag)
    | project-away fft_y_real, fft_y_imag   //  too many series for linechart with panels
    | render linechart with(ysplit=panels)
    ```
    
    This query returns *y_real2* and *y_imag2, which are the same as *y_real* and *y_imag*:  
    
    :::image type="content" source="images/series-fft-function/series-ifft.png" alt-text="Series ifft" border="false":::
    