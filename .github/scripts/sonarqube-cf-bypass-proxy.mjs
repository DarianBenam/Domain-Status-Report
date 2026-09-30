import http from "http";
import https from "https";
import { URL } from "url";

if (!process.env.SONAR_UPSTREAM_URL) {
    console.error("SONAR_UPSTREAM_URL environment variable is required.");
    process.exit(1);
}

if (!process.env.SONAR_PROXY_HOST_URL) {
    console.error("SONAR_PROXY_HOST_URL environment variable is required.");
    process.exit(1);
}

if (!process.env.CF_BYPASS_TOKEN) {
    console.error("CF_BYPASS_TOKEN environment variable is required.");
    process.exit(1);
}

const listenUrl = new URL(process.env.SONAR_PROXY_HOST_URL);
const listenHost = listenUrl.hostname;
const listenPort = Number(listenUrl.port);
const target = new URL(process.env.SONAR_UPSTREAM_URL);
const cloudflareBypassToken = process.env.CF_BYPASS_TOKEN;

const agent = new https.Agent({ keepAlive: true });

const server = http.createServer((request, response) => {
    const headers = {
        ...request.headers,
        host: target.host
    };

    headers["X-CF-Bypass-Token"] = cloudflareBypassToken;
    headers["X-Forwarded-By"] = "SonarQube-CF-Bypass-Proxy";

    delete headers.connection;

    const proxyRequest = https.request(
        {
            protocol: target.protocol,
            hostname: target.hostname,
            port: target.port || 443,
            path: request.url,
            method: request.method,
            headers,
            agent
        },
        (proxyResponse) => {
            response.writeHead(proxyResponse.statusCode || 502, proxyResponse.headers);
            proxyResponse.pipe(response);
        }
    );

    proxyRequest.on("error", (error) => {
        console.error("Proxy request failed:", error.message);

        if (!response.headersSent) {
            response.writeHead(502, { "Content-Type": "text/plain" });
        }

        response.end(`Bad gateway: ${error.message}`);
    });

    request.pipe(proxyRequest);
});

server.listen(listenPort, listenHost, () => {
    console.log(
        `SonarQube Cloudflare bypass proxy listening on: ${listenUrl.origin} -> ${target.origin}`
    );
});
