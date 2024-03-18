
function PaintRBT(rbt) {
    try {
        document.getElementById(rbt).style.background = '#ff0000';
    } catch (e) {
        console.error('CRASH Paint RBT: ', e.message);
    }
}