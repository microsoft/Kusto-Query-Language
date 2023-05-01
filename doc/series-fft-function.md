---
title: series_fft() - Azure Data Explorer
description: Learn how to use the series_fft() function to apply the Fast Fourier Transform (FFT) on a series.
ms.reviewer: adieldar
ms.topic: reference
ms.date: 01/22/2023
---
# series_fft()

Applies the Fast Fourier Transform (FFT) on a series.  

The series_fft() function takes a series of complex numbers in the time/spatial domain and transforms it to the frequency domain using the [Fast Fourier Transform](https://en.wikipedia.org/wiki/Fast_Fourier_transform). The transformed complex series represents the magnitude and phase of the frequencies appearing in the original series. Use the complementary function [series_ifft](series-ifft-function.md) to transform from the frequency domain back to the time/spatial domain.

## Syntax

`series_fft(`*x_real* [`,` *x_imaginary*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *x_real* | dynamic | &check; | A numeric array representing the real component of the series to transform.|
| *x_imaginary* | dynamic | | A similar array representing the imaginary component of the series. This parameter should only be specified if the input series contains complex numbers.|

## Returns

The function returns the complex inverse fft in two series. The first series for the real component and the second one for the imaginary component.

## Example

* Generate a complex series, where the real and imaginary components are pure sine waves in different frequencies. Use FFT to transform it to the frequency domain:

    > [!div class="nextstepaction"]
    > <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1WP3XKDIBCF732KcwmtjZreZJrhWRxaV2WK6ABpsD/v3jXGJt0bOIfvLLuWIoJxdNYfpER6QTOeXi3lmMibsbnpThu3KVXtSiZ6HejPKnelxFcGrgXFw9JW7PmcjJAorg1Zi4THNSxl9pMdM8szOFXtD0dcqijA4c5pC0uui33mtesICa0fB5SIIxyeUCFEmlBl36AUyTWYvdqWESnHQeaYzT/reS8ZD6dh0N58ck816HeqrQlRpAWvPWl7Z87+4ppBd/eukbdfRdvGeg3mWO8LLqEQeGkKNZtiA66PHPccJg/L47312kecTezFHCZropq0IxvkLxVC14SgAQAA" target="_blank">Run the query</a>

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

    :::image type="content" source="images/series-fft-function/series-fft.png" alt-text="Series fft." border="false":::

* Transform a series to the frequency domain, and then apply the inverse transform to get back the original series:

    > [!div class="nextstepaction"]
    > <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3VQ23KDIBB99yvOI7QmUfuSaYZvcWiyKi2CA6TRXv69GLWxnXZfYM+NZTUFeGXoIl9JsP4RJ3t+0pSiI6fs6dbXUpmlE/k2i4pGevqGsm3G8Z4g1ijF3RjLinh2inHs5sDYsx73k5nz5DM5JDrOYERe7A+41m6HaK6N1NBk6tAkTpqa0KNytkWGYGGwQQ4fqEOefID6QOaEwYnlM6xPsecpBvUDeih4lPtz20qn3mKmaOULlVr5wPpRXjqSegUO7oqqVtZrVPHbq6yqQjkZU0z3Uc4h4OOnyZcRZItgJlf2iSkWioqVU43Wf/JjROfsMx3DRl7kgL9l80KDtWilGeZgVNZBx8UcG+kCLio06KQh7WOqi2PRb5oNvtMqiEnFvwD5H9DbOQIAAA==" target="_blank">Run the query</a>

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

    :::image type="content" source="images/series-fft-function/series-ifft.png" alt-text="Series ifft." border="false":::
