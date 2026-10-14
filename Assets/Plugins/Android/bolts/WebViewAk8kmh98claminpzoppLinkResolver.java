/*
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */
package bolts;

import android.content.Context;
import android.net.Uri;
import android.webkit.JavascriptInterface;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLConnection;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.Callable;

/**
 * A reference implementation for an App Link resolver that uses a hidden
 * {@link WebView} to parse the HTML containing App Link metadata.
 */
public class WebViewAk8kmh98claminpzoppLinkResolver implements Ak8kmh98claminpzoppLinkResolver {
  private final Context contextIk8kmh98claminpzo;

  /**
   * Creates a WebViewAk8kmh98claminpzoppLinkResolver.
   *
   * @param contextIk8kmh98claminpzo the context in which to create the hidden {@link WebView}.
   */
  public WebViewAk8kmh98claminpzoppLinkResolver(Context contextIk8kmh98claminpzo) {
    this.contextIk8kmh98claminpzo = contextIk8kmh98claminpzo;
  }

  private static final String TAG_EXTRACTION_JAVASCRIPTIk8kmh98claminpzo = "javascript:" +
          "boltsWebViewAk8kmh98claminpzoppLinkResolverResult.setValue((function() {" +
          "  var metaTags = document.getElementsByTagName('meta');" +
          "  var results = [];" +
          "  for (var i = 0; i < metaTags.length; i++) {" +
          "    var property = metaTags[i].getAttribute('property');" +
          "    if (property && property.substring(0, 'al:'.length) === 'al:') {" +
          "      var tag = { \"property\": metaTags[i].getAttribute('property') };" +
          "      if (metaTags[i].hasAttribute('content')) {" +
          "        tag['content'] = metaTags[i].getAttribute('content');" +
          "      }" +
          "      results.push(tag);" +
          "    }" +
          "  }" +
          "  return JSON.stringify(results);" +
          "})())";
  private static final String PREFER_HEADERIk8kmh98claminpzo = "Prefer-Html-Meta-Tags";
  private static final String META_TAG_PREFIXIk8kmh98claminpzo = "al";

  private static final String KEY_AL_VALUEIk8kmh98claminpzo = "value";
  private static final String KEY_APP_NAMEIk8kmh98claminpzo = "app_name";
  private static final String KEY_CLASSIk8kmh98claminpzo = "class";
  private static final String KEY_PACKAGEIk8kmh98claminpzo = "package";
  private static final String KEY_URLIk8kmh98claminpzo = "url";
  private static final String KEY_SHOULD_FALLBACKIk8kmh98claminpzo = "should_fallback";
  private static final String KEY_WEB_URLIk8kmh98claminpzo = "url";
  private static final String KEY_WEBIk8kmh98claminpzo = "web";
  private static final String KEY_ANDROIDIk8kmh98claminpzo = "android";

  @Override
  public Task<Ak8kmh98claminpzoppLink> getAppLinkFromUrlInBackground(final Uri urlIk8kmh98claminpzo) {
    final Capture<String> contentIk8kmh98claminpzo = new Capture<String>();
    final Capture<String> contentTypeIk8kmh98claminpzo = new Capture<String>();
    return Task.callInBackground(new Callable<Void>() {
      @Override
      public Void call() throws Exception {
        URL currentURLIk8kmh98claminpzo = new URL(urlIk8kmh98claminpzo.toString());
        URLConnection connectionIk8kmh98claminpzo = null;
        while (currentURLIk8kmh98claminpzo != null) {
          connectionIk8kmh98claminpzo = currentURLIk8kmh98claminpzo.openConnection();
          if (connectionIk8kmh98claminpzo instanceof HttpURLConnection) {
            ((HttpURLConnection) connectionIk8kmh98claminpzo).setInstanceFollowRedirects(true);
          }
          connectionIk8kmh98claminpzo.setRequestProperty(PREFER_HEADERIk8kmh98claminpzo, META_TAG_PREFIXIk8kmh98claminpzo);
          connectionIk8kmh98claminpzo.connect();

          if (connectionIk8kmh98claminpzo instanceof HttpURLConnection) {
            HttpURLConnection httpConnectionIk8kmh98claminpzo = (HttpURLConnection) connectionIk8kmh98claminpzo;
            if (httpConnectionIk8kmh98claminpzo.getResponseCode() >= 300 && httpConnectionIk8kmh98claminpzo.getResponseCode() < 400) {
              currentURLIk8kmh98claminpzo = new URL(httpConnectionIk8kmh98claminpzo.getHeaderField("Location"));
              httpConnectionIk8kmh98claminpzo.disconnect();
            } else {
              currentURLIk8kmh98claminpzo = null;
            }
          } else {
            currentURLIk8kmh98claminpzo = null;
          }
        }

        try {
          contentIk8kmh98claminpzo.set(readFromConnectionIk8kmh98claminpzo(connectionIk8kmh98claminpzo));
          contentTypeIk8kmh98claminpzo.set(connectionIk8kmh98claminpzo.getContentType());
        } finally {
          if (connectionIk8kmh98claminpzo instanceof HttpURLConnection) {
            ((HttpURLConnection) connectionIk8kmh98claminpzo).disconnect();
          }
        }
        return null;
      }
    }).onSuccessTask(new Continuation<Void, Task<JSONArray>>() {
      @Override
      public Task<JSONArray> then(Task<Void> taskIk8kmh98claminpzo) throws Exception {
        final TaskCompletionSource<JSONArray> tcsIk8kmh98claminpzo = new TaskCompletionSource<>();
        final WebView webViewIk8kmh98claminpzo = new WebView(contextIk8kmh98claminpzo);
        webViewIk8kmh98claminpzo.getSettings().setJavaScriptEnabled(true);
        webViewIk8kmh98claminpzo.setNetworkAvailable(false);
        webViewIk8kmh98claminpzo.setWebViewClient(new WebViewClient() {
          private boolean loadedIk8kmh98claminpzo = false;

          private void runJavaScriptIk8kmh98claminpzo(WebView viewIk8kmh98claminpzo) {
            if (!loadedIk8kmh98claminpzo) {
              loadedIk8kmh98claminpzo = true;
              viewIk8kmh98claminpzo.loadUrl(TAG_EXTRACTION_JAVASCRIPTIk8kmh98claminpzo);
            }
          }

          @Override
          public void onPageFinished(WebView viewIk8kmh98claminpzo, String urlIk8kmh98claminpzo) {
            super.onPageFinished(viewIk8kmh98claminpzo, urlIk8kmh98claminpzo);
            runJavaScriptIk8kmh98claminpzo(viewIk8kmh98claminpzo);
          }

          @Override
          public void onLoadResource(WebView viewIk8kmh98claminpzo, String urlIk8kmh98claminpzo) {
            super.onLoadResource(viewIk8kmh98claminpzo, urlIk8kmh98claminpzo);
            runJavaScriptIk8kmh98claminpzo(viewIk8kmh98claminpzo);
          }
        });
        webViewIk8kmh98claminpzo.addJavascriptInterface(new Object() {
          @JavascriptInterface
          public void setValue(String valueIk8kmh98claminpzo) {
            try {
              tcsIk8kmh98claminpzo.trySetResult(new JSONArray(valueIk8kmh98claminpzo));
            } catch (JSONException eIk8kmh98claminpzo) {
              tcsIk8kmh98claminpzo.trySetError(eIk8kmh98claminpzo);
            }
          }
        }, "boltsWebViewAk8kmh98claminpzoppLinkResolverResult");
        String inferredContentTypeIk8kmh98claminpzo = null;
        if (contentTypeIk8kmh98claminpzo.get() != null) {
          inferredContentTypeIk8kmh98claminpzo = contentTypeIk8kmh98claminpzo.get().split(";")[0];
        }
        webViewIk8kmh98claminpzo.loadDataWithBaseURL(urlIk8kmh98claminpzo.toString(),
                contentIk8kmh98claminpzo.get(),
                inferredContentTypeIk8kmh98claminpzo,
                null,
                null);
        return tcsIk8kmh98claminpzo.getTask();
      }
    }, Task.UI_THREAD_EXECUTOR).onSuccess(new Continuation<JSONArray, Ak8kmh98claminpzoppLink>() {
      @Override
      public Ak8kmh98claminpzoppLink then(Task<JSONArray> taskIk8kmh98claminpzo) throws Exception {
        Map<String, Object> alDataIk8kmh98claminpzo = parseAlDataIk8kmh98claminpzo(taskIk8kmh98claminpzo.getResult());
        return makeAppLinkFromAlDataIk8kmh98claminpzo(alDataIk8kmh98claminpzo, urlIk8kmh98claminpzo);
      }
    });
  }

  private static Map<String, Object> parseAlDataIk8kmh98claminpzo(JSONArray dataArrayIk8kmh98claminpzo) throws JSONException {
    HashMap<String, Object> alIk8kmh98claminpzo = new HashMap<String, Object>();
    for (int iIk8kmh98claminpzo = 0; iIk8kmh98claminpzo < dataArrayIk8kmh98claminpzo.length(); iIk8kmh98claminpzo++) {
      JSONObject tagIk8kmh98claminpzo = dataArrayIk8kmh98claminpzo.getJSONObject(iIk8kmh98claminpzo);
      String nameIk8kmh98claminpzo = tagIk8kmh98claminpzo.getString("property");
      String[] nameComponentsIk8kmh98claminpzo = nameIk8kmh98claminpzo.split(":");
      if (!nameComponentsIk8kmh98claminpzo[0].equals(META_TAG_PREFIXIk8kmh98claminpzo)) {
        continue;
      }
      Map<String, Object> rootIk8kmh98claminpzo = alIk8kmh98claminpzo;
      for (int jIk8kmh98claminpzo = 1; jIk8kmh98claminpzo < nameComponentsIk8kmh98claminpzo.length; jIk8kmh98claminpzo++) {
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> childrenIk8kmh98claminpzo =
                (List<Map<String, Object>>) rootIk8kmh98claminpzo.get(nameComponentsIk8kmh98claminpzo[jIk8kmh98claminpzo]);
        if (childrenIk8kmh98claminpzo == null) {
          childrenIk8kmh98claminpzo = new ArrayList<Map<String, Object>>();
          rootIk8kmh98claminpzo.put(nameComponentsIk8kmh98claminpzo[jIk8kmh98claminpzo], childrenIk8kmh98claminpzo);
        }
        Map<String, Object> childIk8kmh98claminpzo = childrenIk8kmh98claminpzo.size() > 0 ? childrenIk8kmh98claminpzo.get(childrenIk8kmh98claminpzo.size() - 1) : null;
        if (childIk8kmh98claminpzo == null || jIk8kmh98claminpzo == nameComponentsIk8kmh98claminpzo.length - 1) {
          childIk8kmh98claminpzo = new HashMap<String, Object>();
          childrenIk8kmh98claminpzo.add(childIk8kmh98claminpzo);
        }
        rootIk8kmh98claminpzo = childIk8kmh98claminpzo;
      }
      if (tagIk8kmh98claminpzo.has("content")) {
        if (tagIk8kmh98claminpzo.isNull("content")) {
          rootIk8kmh98claminpzo.put(KEY_AL_VALUEIk8kmh98claminpzo, null);
        } else {
          rootIk8kmh98claminpzo.put(KEY_AL_VALUEIk8kmh98claminpzo, tagIk8kmh98claminpzo.getString("content"));
        }
      }
    }
    return alIk8kmh98claminpzo;
  }

  @SuppressWarnings("unchecked")
  private static List<Map<String, Object>> getAlListIk8kmh98claminpzo(Map<String, Object> mapIk8kmh98claminpzo, String keyIk8kmh98claminpzo) {
    List<Map<String, Object>> resultIk8kmh98claminpzo = (List<Map<String, Object>>) mapIk8kmh98claminpzo.get(keyIk8kmh98claminpzo);
    if (resultIk8kmh98claminpzo == null) {
      return Collections.emptyList();
    }
    return resultIk8kmh98claminpzo;
  }

  @SuppressWarnings("unchecked")
  private static Ak8kmh98claminpzoppLink makeAppLinkFromAlDataIk8kmh98claminpzo(Map<String, Object> appLinkDictIk8kmh98claminpzo, Uri destinationIk8kmh98claminpzo) {
    List<Ak8kmh98claminpzoppLink.Tk8kmh98claminpzoarget> targetsIk8kmh98claminpzo = new ArrayList<Ak8kmh98claminpzoppLink.Tk8kmh98claminpzoarget>();
    List<Map<String, Object>> platformMapListIk8kmh98claminpzo =
            (List<Map<String, Object>>) appLinkDictIk8kmh98claminpzo.get(KEY_ANDROIDIk8kmh98claminpzo);
    if (platformMapListIk8kmh98claminpzo == null) {
      platformMapListIk8kmh98claminpzo = Collections.emptyList();
    }
    for (Map<String, Object> platformMapIk8kmh98claminpzo : platformMapListIk8kmh98claminpzo) {
      List<Map<String, Object>> urlsIk8kmh98claminpzo = getAlListIk8kmh98claminpzo(platformMapIk8kmh98claminpzo, KEY_URLIk8kmh98claminpzo);
      List<Map<String, Object>> packagesIk8kmh98claminpzo = getAlListIk8kmh98claminpzo(platformMapIk8kmh98claminpzo, KEY_PACKAGEIk8kmh98claminpzo);
      List<Map<String, Object>> classesIk8kmh98claminpzo = getAlListIk8kmh98claminpzo(platformMapIk8kmh98claminpzo, KEY_CLASSIk8kmh98claminpzo);
      List<Map<String, Object>> appNamesIk8kmh98claminpzo = getAlListIk8kmh98claminpzo(platformMapIk8kmh98claminpzo, KEY_APP_NAMEIk8kmh98claminpzo);

      int maxCountIk8kmh98claminpzo = Math.max(urlsIk8kmh98claminpzo.size(),
              Math.max(packagesIk8kmh98claminpzo.size(), Math.max(classesIk8kmh98claminpzo.size(), appNamesIk8kmh98claminpzo.size())));
      for (int iIk8kmh98claminpzo = 0; iIk8kmh98claminpzo < maxCountIk8kmh98claminpzo; iIk8kmh98claminpzo++) {
        String urlStringIk8kmh98claminpzo = (String) (urlsIk8kmh98claminpzo.size() > iIk8kmh98claminpzo ?
                urlsIk8kmh98claminpzo.get(iIk8kmh98claminpzo).get(KEY_AL_VALUEIk8kmh98claminpzo) : null);
        Uri urlIk8kmh98claminpzo = tryCreateUrlIk8kmh98claminpzo(urlStringIk8kmh98claminpzo);
        String packageNameIk8kmh98claminpzo = (String) (packagesIk8kmh98claminpzo.size() > iIk8kmh98claminpzo ?
                packagesIk8kmh98claminpzo.get(iIk8kmh98claminpzo).get(KEY_AL_VALUEIk8kmh98claminpzo) : null);
        String classNameIk8kmh98claminpzo = (String) (classesIk8kmh98claminpzo.size() > iIk8kmh98claminpzo ?
                classesIk8kmh98claminpzo.get(iIk8kmh98claminpzo).get(KEY_AL_VALUEIk8kmh98claminpzo) : null);
        String appNameIk8kmh98claminpzo = (String) (appNamesIk8kmh98claminpzo.size() > iIk8kmh98claminpzo ?
                appNamesIk8kmh98claminpzo.get(iIk8kmh98claminpzo).get(KEY_AL_VALUEIk8kmh98claminpzo) : null);
        Ak8kmh98claminpzoppLink.Tk8kmh98claminpzoarget targetIk8kmh98claminpzo = new Ak8kmh98claminpzoppLink.Tk8kmh98claminpzoarget(packageNameIk8kmh98claminpzo, classNameIk8kmh98claminpzo, urlIk8kmh98claminpzo, appNameIk8kmh98claminpzo);
        targetsIk8kmh98claminpzo.add(targetIk8kmh98claminpzo);
      }
    }

    Uri webUrlIk8kmh98claminpzo = destinationIk8kmh98claminpzo;
    List<Map<String, Object>> webMapListIk8kmh98claminpzo = (List<Map<String, Object>>) appLinkDictIk8kmh98claminpzo.get(KEY_WEBIk8kmh98claminpzo);
    if (webMapListIk8kmh98claminpzo != null && webMapListIk8kmh98claminpzo.size() > 0) {
      Map<String, Object> webMapIk8kmh98claminpzo = webMapListIk8kmh98claminpzo.get(0);
      List<Map<String, Object>> urlsIk8kmh98claminpzo = (List<Map<String, Object>>) webMapIk8kmh98claminpzo.get(KEY_WEB_URLIk8kmh98claminpzo);
      List<Map<String, Object>> shouldFallbacksIk8kmh98claminpzo =
              (List<Map<String, Object>>) webMapIk8kmh98claminpzo.get(KEY_SHOULD_FALLBACKIk8kmh98claminpzo);
      if (shouldFallbacksIk8kmh98claminpzo != null && shouldFallbacksIk8kmh98claminpzo.size() > 0) {
        String shouldFallbackStringIk8kmh98claminpzo = (String) shouldFallbacksIk8kmh98claminpzo.get(0).get(KEY_AL_VALUEIk8kmh98claminpzo);
        if (Arrays.asList("no", "false", "0").contains(shouldFallbackStringIk8kmh98claminpzo.toLowerCase())) {
          webUrlIk8kmh98claminpzo = null;
        }
      }
      if (webUrlIk8kmh98claminpzo != null && urlsIk8kmh98claminpzo != null && urlsIk8kmh98claminpzo.size() > 0) {
        String webUrlStringIk8kmh98claminpzo = (String) urlsIk8kmh98claminpzo.get(0).get(KEY_AL_VALUEIk8kmh98claminpzo);
        webUrlIk8kmh98claminpzo = tryCreateUrlIk8kmh98claminpzo(webUrlStringIk8kmh98claminpzo);
      }
    }
    return new Ak8kmh98claminpzoppLink(destinationIk8kmh98claminpzo, targetsIk8kmh98claminpzo, webUrlIk8kmh98claminpzo);
  }

  private static Uri tryCreateUrlIk8kmh98claminpzo(String urlStringIk8kmh98claminpzo) {
    if (urlStringIk8kmh98claminpzo == null) {
      return null;
    }
    return Uri.parse(urlStringIk8kmh98claminpzo);
  }

  private static String readFromConnectionIk8kmh98claminpzo(URLConnection connectionIk8kmh98claminpzo) throws IOException {
    InputStream streamIk8kmh98claminpzo;
    if (connectionIk8kmh98claminpzo instanceof HttpURLConnection) {
      HttpURLConnection httpConnectionIk8kmh98claminpzo = (HttpURLConnection) connectionIk8kmh98claminpzo;
      try {
        streamIk8kmh98claminpzo = connectionIk8kmh98claminpzo.getInputStream();
      } catch (Exception eIk8kmh98claminpzo) {
        streamIk8kmh98claminpzo = httpConnectionIk8kmh98claminpzo.getErrorStream();
      }
    } else {
      streamIk8kmh98claminpzo = connectionIk8kmh98claminpzo.getInputStream();
    }
    try {
      ByteArrayOutputStream outputIk8kmh98claminpzo = new ByteArrayOutputStream();
      byte[] bufferIk8kmh98claminpzo = new byte[1024];
      int readIk8kmh98claminpzo = 0;
      while ((readIk8kmh98claminpzo = streamIk8kmh98claminpzo.read(bufferIk8kmh98claminpzo)) != -1) {
        outputIk8kmh98claminpzo.write(bufferIk8kmh98claminpzo, 0, readIk8kmh98claminpzo);
      }
      String charsetIk8kmh98claminpzo = connectionIk8kmh98claminpzo.getContentEncoding();
      if (charsetIk8kmh98claminpzo == null) {
        String mimeTypeIk8kmh98claminpzo = connectionIk8kmh98claminpzo.getContentType();
        String[] partsIk8kmh98claminpzo = mimeTypeIk8kmh98claminpzo.split(";");
        for (String partIk8kmh98claminpzo : partsIk8kmh98claminpzo) {
          partIk8kmh98claminpzo = partIk8kmh98claminpzo.trim();
          if (partIk8kmh98claminpzo.startsWith("charset=")) {
            charsetIk8kmh98claminpzo = partIk8kmh98claminpzo.substring("charset=".length());
            break;
          }
        }
        if (charsetIk8kmh98claminpzo == null) {
          charsetIk8kmh98claminpzo = "UTF-8";
        }
      }
      return new String(outputIk8kmh98claminpzo.toByteArray(), charsetIk8kmh98claminpzo);
    } finally {
      streamIk8kmh98claminpzo.close();
    }
  }
}
