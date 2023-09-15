const express = require('express');
const socketio = require('socket.io');
const path = require('path');

const app = express();

app.use(express.static(path.join(__dirname, 'public')));
app.use(express.static(path.join(__dirname, 'public/src')));

const PORT = process.env.PORT || 3000;

const server = app.listen(PORT, () =>
  console.log(`Server running on http://localhost:${PORT}`)
);

const io = socketio(server);

const connectedUsers = [];

io.on('connection', socket => {
  socket.on('session-join', (username, sessionId) => {
    connectedUsers.push({
      userId: socket.id,
      username,
      sessionId,
    });

    socket.join(sessionId);

    socket.emit('message', `Welcome to CodeShare ${username}`);

    socket.broadcast
      .to(sessionId)
      .emit('message', `${username} just joined the session`);

    const sessionUsers = connectedUsers.filter(
      user => user.sessionId === sessionId
    );

    io.to(sessionId).emit('session-users', sessionUsers);
  });

  socket.on('editor-content-change', editorContent => {
    const user = connectedUsers.find(user => user.userId === socket.id);

    if (!user) return;

    socket.broadcast
      .to(user.sessionId)
      .emit('editor-content-change', editorContent);
  });

  socket.on('chat-message', message => {
    const user = connectedUsers.find(user => user.userId === socket.id);

    if (!user) return;

    socket.broadcast
      .to(user.sessionId)
      .emit('chat-message', user.username, message);
  });

  socket.on('disconnect', () => {
    const index = connectedUsers.findIndex(user => user.userId === socket.id);
    let client;

    if (index !== -1) client = connectedUsers.splice(index, 1)[0];

    if (client) {
      io.to(client.sessionId).emit(
        'message',
        `${client.username} has left the session`
      );

      const sessionUsers = connectedUsers.filter(
        user => user.sessionId === client.sessionId
      );

      io.to(client.sessionId).emit('session-users', sessionUsers);
    }
  });
});
