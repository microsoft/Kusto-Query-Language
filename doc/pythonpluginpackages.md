

* Your own private package
* Public package but likely to be irrelevant for other plugin users
* Different version of existing package
* To use it immediately

You can self install packages by zipping them, upload the zip to a blob storage, and reference it from the Python sandbox using the external_artifact plugin parameter as follows:

1. One-time prerequisite:
    * Create a blob container to host the package(s), preferably at the same region as your cluster. For example, https://artifcatswestus.blob.core.windows.net/python (assuming your cluster is in West US)
    * Open a support ticket to enable the sandbox accessing that location. The ticket's subject should be "Altering callout policy for sandbox artifacts" and the description should include the command to run.
        * For example, to enable access to a blob located in https://    artifcatswestus.blob.core.windows.net/python the command to run is:

        <!-- csl -->
        ```
        .alter-merge cluster policy callout @'[{"CalloutType": "sandbox_artifacts","CalloutUriRegex": "artifcatswestus\\.blob\\.core\\.windows\\.net/python/","CanCall": true}]'
        ```
        
        * Note: in the future, setting this will not require opening a ticket but would be a setting in the Azure portal.

2. For public packages (in PyPi or other channels)
    * Download the package and its dependencies.
    * If required, compile to wheel (`*.whl`) files:
        * From a cmd window (in your local Python machine) run:

        ```python
        pip wheel [-w download-dir] package-name.
        ```

3. Create a zip file, containing the required package and its dependencies:

    * For public packages: zip the files that were downloaded in the previous step.
    * Notes:
        * Make sure to zip the `.whl` files themselves and *not* their parent folder.
        * You can skip `.whl` files for packages that already exist with the same version in the base sandbox image.
    * For private packages: zip the folder of the package and those of its dependencies

4. Upload the zipped file to a blob in the artifacts location.

5. Calling the `python` plugin:
    * Add an `external_artifacts` parameter with a property bag of name and reference to the zip file (the blob's URL).
    * In your inline python code: import `Zipackage` from `sandbox_utils` and call its `install()` method with the name of the zip file.

### Example

Installing the [Faker](https://pypi.org/project/Faker/) package that generates fake data:

<!-- csl -->
```
range Id from 1 to 3 step 1 
| extend Name=''
| evaluate python(typeof(*),
    'from sandbox_utils import Zipackage\n'
    'Zipackage.install("Faker.zip")\n'
    'from faker import Faker\n'
    'fake = Faker()\n'
    'result = df\n'
    'for i in range(df.shape[0]):\n'
    '    result.loc[i, "Name"] = fake.name()\n',
    external_artifacts=pack('faker.zip', 'https://artifacts.blob.core.windows.net/kusto/Faker.zip?...'))
```

| Id | Name         |
|----|--------------|
|   1| Gary Tapia   |
|   2| Emma Evans   |
|   3| Ashley Bowen |
<#endif>
